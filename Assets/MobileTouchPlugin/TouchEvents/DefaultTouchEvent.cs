using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MobileNativeTouch
{
	public class DefaultTouchEvent : MonoBehaviour {
		private MobileTouchInfoManager infoManager;
		private List<TouchInfo> touches;

		void Awake()
		{ 
			infoManager = MobileTouch.GetInfoManager;
			touches = new List<TouchInfo> ();
		}

		void Start()
		{
			Input.simulateMouseWithTouches = false;
		}

		void Update()
		{
			for(var i=infoManager.CurrentInfo.Count-1; i>=0; i--) {
				if (infoManager.CurrentInfo[i].phase == TouchPhase.Ended) {
					infoManager.RemoveAt (i);
				}
			}

			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) { 
				InputForTouch (ref touches);
			} else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor){
				InputForTouch (ref touches);
				InputForMouse (ref touches);
			} else {
				InputForMouse (ref touches);
			}

			if (touches.Count > 0) {
				if (touches.Count < infoManager.InfoCount) {
					infoManager.Clear ();
					return;
				}

				foreach (var touch in touches) {
					switch (touch.phase) {
					case TouchPhase.Began:
						infoManager.Add (touch);
						break;
					case TouchPhase.Moved:
						infoManager.Update (touch);
						break;
					case TouchPhase.Ended:
						infoManager.Update (touch);
						break;
					case TouchPhase.Canceled:
						touch.phase = TouchPhase.Ended;
						infoManager.Update (touch);
						break;
					case TouchPhase.Stationary:
						infoManager.Update (touch);
						break;
					}
				}
				touches.Clear ();
			}
		}

		void OnDestroy() 
		{
			infoManager.Clear ();
		}


		private void InputForTouch (ref List<TouchInfo> touchInfo) 
		{
			if (Input.touchCount == 0) 
				return;

			float eventTime = Time.realtimeSinceStartup;
			foreach (var touch in Input.touches) {
				TouchInfo info = new TouchInfo (touch.fingerId, touch.position, eventTime, 0f, 0f);
				info.phase = touch.phase;
				touchInfo.Add (info);
			}
 		}

		private void InputForMouse (ref List<TouchInfo> touchInfo)
		{

			float eventTime = Time.realtimeSinceStartup;

			#if UNITY_EDITOR
			TouchInfo info = new TouchInfo (-1, currentMousePositionInGameView, eventTime, 0f, 0f); 
			#else
			TouchInfo info = new TouchInfo (-1, Input.mousePosition, eventTime, 0f, 0f); 
			#endif

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

		#if UNITY_EDITOR
		private Vector2 currentMousePositionInGameView = Vector2.zero;
		private Vector2 beforeMousePosition = Vector2.zero;
		void OnGUI()
		{
			Vector2 currentMousePosition = Event.current.mousePosition;
			if (currentMousePosition.x == float.MaxValue) {
				currentMousePosition = beforeMousePosition;
			} else {
				beforeMousePosition = currentMousePosition;
			}
			var x = currentMousePosition.x;
			var y = Screen.height - currentMousePosition.y;
			currentMousePosition = new Vector2 (x, y);
			currentMousePositionInGameView = currentMousePosition;
		}
		#endif 
	}
}
