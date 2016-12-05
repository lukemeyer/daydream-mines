using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {
	/*
	public GameObject detectorPrefab;

	public Tile detected;

	private SteamVR_TrackedController controller;
	private SteamVR_LaserPointer pointer;

	// Use this for initialization
	void Start () {
		controller = GetComponent<SteamVR_TrackedController>();
		if (controller == null)
		{
			controller = gameObject.AddComponent<SteamVR_TrackedController>();
		}

		pointer = GetComponent<SteamVR_LaserPointer> ();

		controller.TriggerClicked += new ClickedEventHandler(touchTile);
		controller.PadClicked += new ClickedEventHandler(flagTile);
		controller.MenuButtonClicked += new ClickedEventHandler(reset);

		pointer.PointerIn += new PointerEventHandler (pointed);
		pointer.PointerOut += new PointerEventHandler (unPointed);

		if (detectorPrefab != null) {
			//attachDetector ();
			Debug.Log ("attached");
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void reset(object sender, ClickedEventArgs e){
		if (detected != null) {
			detected._field.setTiles ();
		}
	}

	void flagTile(object sender, ClickedEventArgs e){

		if ( detected != null ) {
			detected.toggleFlagged ();
		}

	}

	void touchTile(object sender, ClickedEventArgs e){
		
		if ( detected != null ) {
			if (detected.hasMine) {
				detected._mine.SetActive (true);
				//detected.display.text = "X";
				detected.status = Tile.BlockStatus.MINED;
				} else {
				detected.cascade (Time.frameCount);
				}
		}
		detected._field.updateScore ();

	}

	void attachDetector(){
		GameObject detector = (GameObject)Instantiate (detectorPrefab, transform.position, transform.rotation);
		detector.transform.parent = transform;
	}

	void pointed(object sender, PointerEventArgs e){
		Tile otherTile = e.target.GetComponent<Tile> ();
		if (otherTile != null) {
			detected = otherTile;
			if (detected.status != Tile.BlockStatus.CLEARED) {
				detected.setActive (true);
			}
		}
	}

	void unPointed(object sender, PointerEventArgs e){
		Tile otherTile = e.target.GetComponent<Tile> ();
		if (otherTile != null) {
			detected = otherTile;
			detected.setActive (false);
		}
	}

	void OnTriggerStay(Collider other){
		//Debug.Log ("enter");
		Tile otherTile = other.GetComponent<Tile> ();
		if ( otherTile != null ) {
			if (detected != null) {
				if (Vector3.Distance (otherTile.transform.position, transform.position) < Vector3.Distance (detected.transform.position, transform.position)) {
					//detected.display.text = "";
					detected = otherTile;
					//detected.display.text = "O";
				}
			} else {
				detected = otherTile;
				//detected.display.text = "O";
			}


		}
	}

	void OnTriggerExit(Collider other){
		//Debug.Log ("exit");
		Tile otherTile = other.GetComponent<Tile> ();
		if ( otherTile != null ) {
			if (detected == otherTile) {
				detected = null;
			}

			//otherTile.display.text = "";
		}
	}
	*/
}
