using UnityEngine;
using System.Collections;

public class Tes : MonoBehaviour, IMobileTouchEventHandler {

	// Use this for initialization
	void Start () {
		MobileTouch.RegisterEventHandler (this);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (MobileTouch.touches.Length);
	}

	#region IMobileTouchEventHandler implementation

	public bool OnTouchEventBegan (TouchInfo[] touchInfo)
	{
		foreach (var info in touchInfo) {
			Debug.Log ("Began: " + info.currentScreenPosition + ", " + info.touchId);
		}

		return false;
	}

	public bool OnTouchEventEnded (TouchInfo[] touchInfo)
	{
		foreach (var info in touchInfo) {
			Debug.Log ("Ended: " + info.currentScreenPosition + ", " + info.touchId);
		}

		return false;
	}

	public bool OnTouchEventMoved (TouchInfo[] touchInfo)
	{
		return false;
	}

	public bool OnTouchEventStationary (TouchInfo[] touchInfo)
	{

		return false;
	}

	#endregion

	#region IInputEventHandler implementation

	public int Order {
		get {
			return 0;
		}
	}

	public bool Process {
		get {
			return true;
		}
	}

	#endregion
}
