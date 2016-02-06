using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MobileNativeTouch
{
	public class DefaultTouchEvent : BaseTouchEvent {
		private bool running;
		private List<TouchInfo> touches;

		void Awake()
		{ 
			touches = new List<TouchInfo> ();
		}

		void Start()
		{
			Input.simulateMouseWithTouches = false;
		}

		void Update()
		{
			for(var i=MobileTouchInfoManager.CurrentInfo.Count-1; i>=0; i--) {
				if (MobileTouchInfoManager.CurrentInfo[i].phase == TouchPhase.Ended) {
					MobileTouchInfoManager.Remove (MobileTouchInfoManager.CurrentInfo[i]);
				}
			}

			#if UNITY_IOS && UNITY_ANDROID
			InputForTouch (ref touches);
			#elif UNITY_EDITOR
			InputForTouch (ref touches);
			InputForMouse (ref touches);
			#else
			InputForMouse (ref touches);
			#endif

			if (touches.Count > 0) {
				foreach (var touch in touches) {
					switch (touch.phase) {
					case TouchPhase.Began:
						MobileTouchInfoManager.Add (touch);
						break;
					case TouchPhase.Moved:
						MobileTouchInfoManager.Update (touch);
						break;
					case TouchPhase.Ended:
						MobileTouchInfoManager.Update (touch);
						break;
					case TouchPhase.Canceled:
						touch.phase = TouchPhase.Ended;
						MobileTouchInfoManager.Update (touch);
						break;
					case TouchPhase.Stationary:
						MobileTouchInfoManager.Update (touch);
						break;
					}
				}
				touches.Clear ();
			}
		}

		void OnDestroy() 
		{
			MobileTouchInfoManager.Clear ();
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
