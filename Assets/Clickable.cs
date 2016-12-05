using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour {

	public UnityEvent onTouchDown;
	public UnityEvent onTouchUp;
	public UnityEvent onTapped;

	public UnityEvent onClickDown;
	public UnityEvent onClickUp;
	public UnityEvent onClicked;

	public UnityEvent onPointOver;
	public UnityEvent onPointOff;

	public void TouchDown(){
		if (onTouchDown != null) {
			onTouchDown.Invoke ();
		}
	}

	public void TouchUp (){
		if (onTouchUp != null) {
			onTouchUp.Invoke ();
		}
	}

	public void Tapped (){
		if (onTapped != null) {
			onTapped.Invoke ();
		}
	}

	public void ClickDown(){
		if (onClickDown != null) {
			onClickDown.Invoke ();
		}
	}

	public void ClickUp (){
		if (onClickUp != null) {
			onClickUp.Invoke ();
		}
	}

	public void Clicked (){
		if (onClicked != null) {
			onClicked.Invoke ();
		}
	}

	public void PointOver (){
		if (onPointOver != null) {
			onPointOver.Invoke ();
		}
	}

	public void PointOff (){
		if (onPointOff != null) {
			onPointOff.Invoke ();
		}
	}
}
