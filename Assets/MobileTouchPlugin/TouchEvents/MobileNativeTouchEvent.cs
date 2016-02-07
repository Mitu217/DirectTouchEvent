using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace MobileNativeTouch
{
	/**********************************
	 * Structure for Shared Native C++
	 **********************************/
	[StructLayout(LayoutKind.Sequential)]
	public struct Touch {
		public int touchID;
		public double timestamp;
		public int phase;
		public float posX;
		public float posY;
		public float radius;
		public float pressure;
	};

	public class MobileNativeTouchEvent : MonoBehaviour
	{
		private delegate void TouchEventCallback(IntPtr ptrTouchInfo, int nVal);

		private static MobileTouchInfoManager infoManager;

		public MobileNativeTouchEvent()
		{
			infoManager = MobileTouch.GetInfoManager;
		}

		void Start() {
			InitializationManager ();
			RegisterTouchEventCallback (Callback);
			EnableNativePlugin ();
		}

		void Update()
		{
			for(var i= infoManager.InfoCount-1; i>=0; i--) {
				if (infoManager.CurrentInfo [i].phase == TouchPhase.Began) {
					TouchInfo touchInfo = infoManager.CurrentInfo [i];
					touchInfo.phase = TouchPhase.Moved;
					infoManager.Update (touchInfo);
				} else if (infoManager.CurrentInfo[i].phase == TouchPhase.Ended) {
					infoManager.RemoveAt (i);
				}
			}
		}

		void OnDestroy()
		{
			DisableNativePlugin ();
		}

		public void EnableDefaultTouchEvent()
		{
			EnableDefaultTouch ();
		}

		public void DisableDefaultTouchEvent()
		{
			DisableDefaultTouch ();
		}



		#if UNITY_IPHONE
		[DllImport ("__Internal")]
		#elif UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		private static extern int InitializationManager ();

		#if UNITY_IPHONE
		[DllImport ("__Internal")]
		#elif UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		private static extern int RegisterTouchEventCallback (TouchEventCallback callback);

		#if UNITY_IPHONE
		[DllImport ("__Internal")]
		#elif UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		private static extern void EnableDefaultTouch ();

		#if UNITY_IPHONE
		[DllImport ("__Internal")]
		#elif UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		private static extern void DisableDefaultTouch ();

		#if UNITY_IPHONE
		[DllImport ("__Internal")]
		#elif UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		private static extern void EnableNativePlugin ();

		#if UNITY_IPHONE
		[DllImport ("__Internal")]
		#elif UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		private static extern void DisableNativePlugin ();

		[AOT.MonoPInvokeCallbackAttribute(typeof(TouchEventCallback))]
		static void Callback(IntPtr ptrTouchInfo, int nVal) { 
			if (nVal == 0) return;

			IntPtr ptr = ptrTouchInfo;
			for (var i = 0; i < nVal; i++) {
				MobileNativeTouch.Touch touch = (MobileNativeTouch.Touch)Marshal.PtrToStructure (ptr, typeof(MobileNativeTouch.Touch));
				TouchInfo touchInfo = new TouchInfo (touch);
				switch (touch.phase) {
				case 0: // Began
					touchInfo.phase = TouchPhase.Began;
					infoManager.Add (touchInfo);
					break;
				case 1: // Moved
					touchInfo.phase = TouchPhase.Moved;
					infoManager.Update (touchInfo);
					break;
				case 2: // Stationary
					touchInfo.phase = TouchPhase.Moved;
					infoManager.Update (touchInfo);
					break;
				case 3: // Ended
					touchInfo.phase = TouchPhase.Ended;
					infoManager.Update (touchInfo);
					break;
				case 4: //Cancel
					touchInfo.phase = TouchPhase.Ended;
					infoManager.Update (touchInfo);
					break;
				}


				if (i < nVal - 1) {
				ptr = (IntPtr)((int)ptr + Marshal.SizeOf (typeof(MobileNativeTouch.Touch)));
				}
			}
		}


	}
}
