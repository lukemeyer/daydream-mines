using UnityEngine;
using System.Collections;

namespace Mines
{
	public class MenuButton : MonoBehaviour
	{

		public GameObject ActiveIndicator;

		private MeshRenderer rend;
		public Material ActiveMaterial;
		private Material InactiveMaterial;

		public bool stickyActive = false;
		public string key = "";

		void Awake(){
			rend = GetComponent<MeshRenderer> ();
			InactiveMaterial = rend.material;
		}

		public void setActiveState (bool active)
		{
			Debug.Log ("Setting active state of " + key + " to " + active.ToString ());
			if (ActiveIndicator != null) {
				//Debug.Log ("Setting button " + key + " " + active.ToString ());
				ActiveIndicator.SetActive (stickyActive ? true : active);
			}
			if (ActiveMaterial != null) {
				if (stickyActive || active) {
					rend.material = ActiveMaterial;
				} else {
					rend.material = InactiveMaterial;
				}
			}
		}

		public void setActive ()
		{
			setActiveState (true);
		}

		public void setInactive ()
		{
			setActiveState (false);
		}
	}
}