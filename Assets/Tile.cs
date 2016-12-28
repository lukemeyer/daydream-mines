using UnityEngine;
using System.Collections;

namespace Mines
{
	public class Tile : MonoBehaviour
	{

		public BlockField _field;

		public enum BlockStatus
		{
			UNTOUCHED,
			DISPLAY,
			CLEARED,
			FLAGGED,
			UNFLAGGED,
			MINED}

		;

		public Material standardMaterial;
		public Material activeMaterial;

		public Renderer mineRenderer;
		public Material standardMineMaterial;
		public Material activeMineMaterial;

		public BoxCollider TileCollider;

		public GameObject _block;
		//private GameObject _tile;
		public GameObject _mine;
		public GameObject _flag;

		public Rigidbody rbody;

		public Tile.BlockStatus status = BlockStatus.UNTOUCHED;
		public bool hasMine = false;
		public bool flagged = false;

		public SimpleHelvetica display;

		public int nearby = 0;
		public Tile[] neighbors;

		public Vector3 clearedPosition;
		public Vector3 untouchedPosition;

		public int xPosition;
		public int yPosition;

		private int cascadeTime = 0;

		public bool isFlashing = false;
		private float flashStartTime = 0f;
		private float flashDuration = 3f;
		private float flashSpeed = 1f;

		public void reset ()
		{
			flashSpeed = 1f;
			mineRenderer.material = standardMineMaterial;
			isFlashing = false;
			TileCollider.enabled = true;
			rbody.isKinematic = true;
			setText ("");
			_block.SetActive (true);
			_block.transform.localPosition = untouchedPosition;
			status = BlockStatus.UNTOUCHED;
			hasMine = false;
			_mine.SetActive (false);
			flagged = false;
			_flag.SetActive (flagged);
			setActive (false);
			_mine.transform.rotation = Random.rotation;
		}

		void Awake ()
		{
			rbody = GetComponent<Rigidbody> ();
		}

		void Start ()
		{
		
			//_tile = gameObject;
			_block.transform.localPosition = untouchedPosition;
			setText ("");
		}

		void Update ()
		{
			if (isFlashing) {
				if (Mathf.PingPong (Time.time - flashStartTime, .5f) > .25f) {
					mineRenderer.material = activeMineMaterial;
					flashSpeed = flashSpeed * .5f;
				} else {
					mineRenderer.material = standardMineMaterial;
				}
			}
		}

		public void onPointOver ()
		{
			if (_field.manager.gameHasStarted) {
				if (status != BlockStatus.CLEARED && status != BlockStatus.MINED) {
					setActive (true);
				}
			}
		}

		public void onPointOff ()
		{
			if (_field.manager.gameHasStarted) {
				setActive (false);
			}
		}

		public void onTapped ()
		{
			if (_field.manager.gameHasStarted) {
				toggleFlagged ();
			}
		}

		public void onClicked ()
		{
			if (_field.manager.gameHasStarted) {
				// Log ("Activate Tile: " + activateTarget.xPosition + ", " + activateTarget.zPosition);
				if (status != Tile.BlockStatus.FLAGGED) {
					if (hasMine) {
						_mine.SetActive (true);
						status = Tile.BlockStatus.MINED;
					} else {
						cascade (Time.frameCount);
					}
					_field.updateScore ();
				}
			}
		}

		public void setText (string newText)
		{
			if (newText.Length > 0) {
				display.gameObject.SetActive (true);
				display.Text = newText;
				display.GenerateText ();
			} else {
				display.gameObject.SetActive (false);
			}
		}

		public void setActive (bool active)
		{
		
			if (active) {
				_block.GetComponent<Renderer> ().material = activeMaterial;
			} else {
				_block.GetComponent<Renderer> ().material = standardMaterial;
			}

		}

		public void cascade (int time)
		{
			if (time != cascadeTime) {
				if (!hasMine) {
					_block.transform.localPosition = clearedPosition;
					status = BlockStatus.CLEARED;
					if (flagged) {
						toggleFlagged ();
					}
					if (nearby == 0) {
						for (int i = 0; i < neighbors.Length; i++) {
							if (neighbors [i].status != BlockStatus.CLEARED) {
								neighbors [i].cascade (time);
							}
						}
					} else {
						setText (nearby.ToString ());
					}
				}
				cascadeTime = time;
			}
		}

		public void toggleFlagged ()
		{
			flagged = status == BlockStatus.CLEARED || status == BlockStatus.DISPLAY ? false : !flagged;
			_flag.SetActive (flagged);
			status = flagged ? BlockStatus.FLAGGED : BlockStatus.UNFLAGGED;
		}

		public void setMine (bool mine)
		{
			hasMine = mine;
		}

		public void blowMine ()
		{
			flashStartTime = Time.time;
			isFlashing = true;
			//TileCollider.enabled = false;
			_flag.SetActive (false);
			_block.SetActive (false);
			_mine.SetActive (true);
			//rbody.isKinematic = false;
		}

		public void updateNeighbors ()
		{
			Tile[] tempNeighbors = new Tile[8];
			int count = 0;
			//find surrounding tiles
			for (int x = -1; x < 2; x++) {
				for (int z = -1; z < 2; z++) {
					int xTestPosition = xPosition + x;
					int zTestPosition = yPosition + z;

					if (!(xTestPosition == xPosition && zTestPosition == yPosition)) {
						//Debug.Log ("+++ " + xTestPosition + ", " + zTestPosition);
						if (xTestPosition > -1 && xTestPosition < _field.fieldWidth && zTestPosition > -1 && zTestPosition < _field.fieldHeight) {
							count++;
							tempNeighbors [count - 1] = _field.getTile (xTestPosition, zTestPosition);
						}
					}
				}
			}
			if (count > 0) {
				neighbors = new Tile[count];
				nearby = 0;
				for (int i = 0; i < count; i++) {
					neighbors [i] = tempNeighbors [i];
					if (neighbors [i].hasMine) {
						nearby++;
					}
				}
			}
		}

		public void updateDisplay ()
		{
			int count = 0;
			for (int i = 0; i < neighbors.Length; i++) {
				if (neighbors [i].hasMine) {
					count++;
				}
			}
			nearby = count;
			if (status == BlockStatus.DISPLAY) {
				setText (count.ToString ());
			} else {
				setText ("");
			}
		}
	}
}