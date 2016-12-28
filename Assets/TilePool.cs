using UnityEngine;
using System.Collections;

namespace Mines
{
	public class TilePool : MonoBehaviour
	{

		public GameObject prefab;
		public int intialCapacity = 100;

		private Stack available;
		private ArrayList all;

		public void Init ()
		{
			Debug.Log ("Starting Pool");
			available = new Stack (intialCapacity);
			all = new ArrayList (intialCapacity);
			FillPool (intialCapacity);
		}

		public Tile GetTile ()
		{
			Tile result;

			if (available == null || available.Count == 0) {
				result = ((GameObject)Instantiate (prefab, Vector3.zero, Quaternion.identity)).GetComponent<Tile> ();
				result.reset ();
				all.Add (result);
			} else {
				result = (Tile)available.Pop ();
				result.gameObject.SetActive (true);
			}

			return result;
		}

		public bool ReturnTile (Tile returning)
		{
			returning.reset ();
			if (!available.Contains (returning)) {
				available.Push (returning);
				returning.gameObject.SetActive (false);
				return true;
			}
			return false;
		}

		public void FillPool (int capacity)
		{
			Debug.Log ("Filling pool with " + capacity + " tiles.");
			Tile[] temp = new Tile[capacity];
			for (int i = 0; i < temp.Length; i++) {
				temp [i] = GetTile ();
				temp [i].gameObject.SetActive (false);
			}
			for (int i = 0; i < temp.Length; i++) {
				ReturnTile (temp [i]);
			}
		}

		public void ReturnAll ()
		{
			for (int i = 0; i < all.Count; i++) {
				ReturnTile ((Tile)all [i]);
			}
		}

	}
}