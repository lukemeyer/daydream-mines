using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public GameObject ActiveIndicator;
	public bool stickyActive = false;
	public string key = "";

	public void setActiveState (bool active){
		if (ActiveIndicator != null) {
			//Debug.Log ("Setting button " + key + " " + active.ToString ());
			ActiveIndicator.SetActive (stickyActive ? true : active);
		}
	}

	public void setActive(){
		setActiveState (true);
	}

	public void setInactive(){
		setActiveState (false);
	}
}
