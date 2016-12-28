using UnityEngine;
using System.Collections;

namespace Mines
{
	public class BlockField : MonoBehaviour
	{

		public SweeperManager manager;

		public GameObject BlockPrefab;
		public GameObject Rig;
		public SimpleHelvetica headDisplay;

		public Transform Origin;
		public Transform TextCenter;

		public TilePool pool;

		private Tile[] tiles;

		public int totalMines = 1;

		public int fieldWidth;
		public int fieldHeight;
		public float mineDensity;

		private const float PLAY_AREA_SIZE = 4f;

		private const int MAX_WIDTH = 10;
		private const int MAX_HEIGHT = 10;

		private const int MIN_WIDTH = 3;
		private const int MIN_HEIGHT = 3;

		public void Init ()
		{
			setTiles ();
		}

		public void setSize (int[] dimensions)
		{
			fieldWidth = Mathf.Clamp (dimensions [0], MIN_WIDTH, MAX_WIDTH);
			fieldHeight = Mathf.Clamp (dimensions [1], MIN_HEIGHT, MAX_HEIGHT);
			//Debug.Log ("Setting field size to " + fieldWidth.ToString() + "x" + fieldHeight.ToString());
		}

		public void setMineDensity (float density)
		{
			mineDensity = Mathf.Clamp01 (density);
			//Debug.Log ("Setting mine density to " + density.ToString());
		}

		public void setText (string newText)
		{
			headDisplay.Text = newText;
			headDisplay.GenerateText ();
			//Debug.Log (headDisplay.transform.GetComponent<MeshRenderer> ().bounds.center);
			//Vector3 delta = headDisplay.transform.GetComponent<MeshRenderer> ().bounds.center - TextCenter.position;
			//headDisplay.transform.position = headDisplay.transform.position + delta;
			if (newText.Length > 0) {
				MeshRenderer[] children = headDisplay.transform.GetComponentsInChildren<MeshRenderer> ();
				Vector3 center = Vector3.zero;
				int active = 0;
				for (int i = 1; i < children.Length; i++) {
					if (children [i].gameObject.activeInHierarchy) {
						active++;
						if (center == Vector3.zero) {
							center = children [i].bounds.center;
						} else {
							center += children [i].bounds.center;
						}

					}
				}
				center /= active;
				Vector3 delta = TextCenter.position - center;
				headDisplay.transform.position = headDisplay.transform.position + delta;
			}
		}

		public void setTiles ()
		{
			float start = Time.realtimeSinceStartup;

			// Scale based on the longest side
			float scale = Mathf.Clamp (PLAY_AREA_SIZE / Mathf.Max (fieldWidth, fieldHeight), .2f, .5f);
			float offsetX = 0 - ((scale * fieldWidth / 2f)) + (scale / 2f);
			float offsetY = 0 - ((scale * fieldHeight / 2f)) + scale;

			Origin.localScale = Vector3.one * scale;
			Origin.localPosition = new Vector3 (offsetX, 0f, offsetY);

			setText ("");
			pool.ReturnAll ();

			tiles = new Tile[fieldWidth * fieldHeight];
			for (int x = 0; x < fieldWidth; x++) {
				for (int y = 0; y < fieldHeight; y++) {
					Tile placedTile = pool.GetTile ();
					placedTile.transform.parent = Origin;
					placedTile.transform.localScale = Vector3.one;
					placedTile.transform.localPosition = new Vector3 (x, 0, y);
					placedTile.transform.localRotation = Quaternion.identity;

					placedTile.xPosition = x;
					placedTile.yPosition = y;
					placedTile._field = this;
					setTile (x, y, placedTile);
				}
			}


			int placedMines = 0;
			totalMines = Mathf.FloorToInt (tiles.Length * mineDensity);
			//Debug.Log ("Set total mines " + totalMines.ToString ());

			// Avoid trying to place more mines than there are tiles
			if (totalMines >= tiles.Length) {
				Debug.Log ("SetTiles exceeded mine limit: " + totalMines + " into " + tiles.Length + " tiles.");
				totalMines = tiles.Length - 1;
			}

			while (placedMines < totalMines) {
				int x = Random.Range (0, fieldWidth);
				int y = Random.Range (0, fieldHeight);
				Tile testTile = getTile (x, y);
				if (!testTile.hasMine) {
					testTile.setMine (true);
					placedMines++;
				}
				if (Time.realtimeSinceStartup - start > 5) {
					Debug.Log ("SetTiles exceeded time limit: " + placedMines + " placed of " + totalMines);
					placedMines = totalMines;
				}
			}

			for (int i = 0; i < tiles.Length; i++) {
				if (tiles [i] != null) {
					tiles [i].updateNeighbors ();
				} else {
					Debug.Log ("Trying to update invalid tile, index " + i.ToString ());
				}
			}
			//Debug.Log ("Set took " + (Time.realtimeSinceStartup - start).ToString () + " seconds");
		}

		public void updateScore ()
		{
			int untouched = 0;
			int flagged = 0;
			for (int i = 0; i < tiles.Length; i++) {
				if (tiles [i].status == Tile.BlockStatus.MINED) {
					//setText ("GAME OVER");
					StartCoroutine (manager.EndGame (false, tiles [i]));
					break;
				} else if (tiles [i].status == Tile.BlockStatus.UNTOUCHED) {
					untouched++;
				} else if (tiles [i].status == Tile.BlockStatus.FLAGGED) {
					flagged++;
				}
			}
			if (untouched + flagged == totalMines) {
				//setText ("WINNER");
				StartCoroutine (manager.EndGame (true, null));
			}
		}

		public void blowMines (Vector3 origin, bool justReveal)
		{
			if (justReveal) {
				for (int i = 0; i < tiles.Length; i++) {
					//tiles [i].rbody.isKinematic = false;
					if (tiles [i].hasMine) {
						tiles [i].blowMine ();
					}
				}
			} else {
				for (int i = 0; i < tiles.Length; i++) {
					tiles [i].rbody.isKinematic = false;
				}
				origin.z = origin.z + Random.Range (-2f, 0f);
				for (int j = 0; j < tiles.Length; j++) {
					if (tiles [j].hasMine) {
						tiles [j].TileCollider.enabled = false;
					}
					tiles [j].isFlashing = false;
					tiles [j].rbody.AddExplosionForce (500f, origin, PLAY_AREA_SIZE);
				}
				/*
			for (int i = 0; i < tiles.Length; i++) {
				if (tiles [i].hasMine) {
					tiles [i].TileCollider.enabled = false;
					tiles [i].isFlashing = false;
					origin = tiles [i].transform.position;
					origin.z = origin.z + Random.Range (-1f, .75f);
					for (int j = 0; j < tiles.Length; j++) {
						tiles [j].rbody.AddExplosionForce (50f, origin, PLAY_AREA_SIZE);
					}

				}
			}
			*/
			}
		}

		private void setTile (int x, int y, Tile tile)
		{
			//Debug.Log ("Setting tile: " + x.ToString() + "," + y.ToString() + " : " + (fieldWidth * y + x).ToString ());
			tiles [fieldWidth * y + x] = tile;
		}

		public Tile getTile (int x, int y)
		{
			return tiles [fieldWidth * y + x];
		}
	}
}