using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Mines
{
	public class Pointer : MonoBehaviour
	{
		public GameObject controllerPivot;
		private Clickable lastPointed;
		private Clickable lastTouched;
		private Clickable lastClicked;

		public UnityEvent onAppButtonUp;

		#if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)

		void Update ()
		{
			UpdatePointer ();
		}

		private void UpdatePointer ()
		{
			if (GvrController.State != GvrConnectionState.Connected) {
				controllerPivot.SetActive (false);
			}
			controllerPivot.SetActive (true);
			controllerPivot.transform.rotation = GvrController.Orientation;

			// Handle pointing
			RaycastHit hitInfo;
			Vector3 rayDirection = GvrController.Orientation * Vector3.forward;

			if (Physics.Raycast (controllerPivot.transform.position, rayDirection, out hitInfo)) {
				if (hitInfo.collider && hitInfo.collider.gameObject) {
					pointed (hitInfo.collider.gameObject);
				}
			} else {
				if (lastPointed != null) {
					unPointed (lastPointed);
				}
			}

			// Handle Tapping
			if (GvrController.TouchDown && lastPointed != null) {
				lastTouched = lastPointed;
				lastTouched.TouchDown ();
			} else if (GvrController.TouchUp && lastPointed != null) {
				lastPointed.TouchUp ();
				if (lastTouched != null && lastTouched == lastPointed) {
					lastTouched.Tapped ();
					lastTouched = null;
				}
			}

			// Handle Clicking
			if (GvrController.ClickButtonDown && lastPointed != null) {
				lastClicked = lastPointed;
				lastPointed.ClickDown ();
			} else if (GvrController.ClickButtonUp && lastPointed != null) {
				lastPointed.ClickUp ();
				if (lastClicked != null && lastClicked == lastPointed) {
					lastClicked.Clicked ();
					lastClicked = null;
				}
			}

			//Handle App Button
			if (GvrController.AppButtonUp) {
				if (onAppButtonUp != null) {
					onAppButtonUp.Invoke ();
				}
			}
		}
		
		#endif  // UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
		/*
		void reset ()
		{
			if (lastTilePointed != null) {
				lastTilePointed._field.setTiles ();
			}
		}
		*/
		// Show hover state on tile
		void pointed (GameObject target)
		{
			// Find Clickable on target
			Clickable pointedClick = target.GetComponent<Clickable> ();
			if (pointedClick != null) {
				// unpoint last Clickable
				if (lastPointed != null) {
					if (lastPointed != pointedClick) {
						lastPointed.PointOff ();
					}
				}
				// point clickable
				pointedClick.PointOver ();
				lastPointed = pointedClick;
			}
		}

		// Remove hover state
		void unPointed (Clickable target)
		{
			target.PointOff ();
		}
	}
}