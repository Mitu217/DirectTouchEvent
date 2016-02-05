using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultTouchEvent : MonoBehaviour {
	private bool running;
	private List<DirectTouchInfo> touchInfo;

	public DefaultTouchEvent() 
	{ 
		running = false;
		touchInfo = new List<DirectTouchInfo> ();
	}

	public void Initialization (DirectTouchEvent touchEvent)
	{
		StartMainLoop (touchEvent);
	}

	public void Finalization()
	{
		StopMainLoop ();
	}

	private void StartMainLoop(DirectTouchEvent touchEvent)
	{
		running = true;
		StartCoroutine (UpdateRoutine(touchEvent));
	}

	private void StopMainLoop()
	{
		running = false;
	}

	IEnumerator UpdateRoutine(DirectTouchEvent touchEvent) 
	{
		while (running) {
			touchInfo.Clear ();

			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
				InputForTouch (ref touchInfo);
			} else {
				InputForMouse (ref touchInfo);
			}

			if (touchInfo.Count > 0) {
				touchEvent.DefaultCallback (touchInfo);
			}

			yield return null;
		}
		yield break;
	}

	private void InputForTouch (ref List<DirectTouchInfo> touchInfo) 
	{
		if (Input.touchCount == 0) 
			return;

		float eventTime = Time.realtimeSinceStartup;
		foreach (var touch in Input.touches) {
			DirectTouchInfo info = new DirectTouchInfo (touch.fingerId, touch.position, eventTime, 0f, 0f);
			info.phase = touch.phase;
			touchInfo.Add (info);
		}
	}

	private void InputForMouse (ref List<DirectTouchInfo> touchInfo)
	{
		float eventTime = Time.realtimeSinceStartup;
		DirectTouchInfo info = new DirectTouchInfo (-1, Input.mousePosition, eventTime, 0f, 0f); 

		if (Input.GetMouseButtonDown (0)) {
			info.phase = TouchPhase.Began;
			touchInfo.Add (info);
		} else if (Input.GetMouseButtonUp (0)) {
			info.phase = TouchPhase.Ended;
			touchInfo.Add (info);
		} else if (Input.GetMouseButton (0)) {
			info.phase = TouchPhase.Moved;
			touchInfo.Add (info);
		}
	}
}
