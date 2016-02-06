using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MobileNativeTouch
{
	public class AndroidTouchEvent : BaseTouchEvent 
	{
		private delegate void TouchEventCallback(IntPtr ptrTouchInfo, int nVal);

		#if UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		private static extern int InitializationManager ();

		#if UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		private static extern int RegisterTouchEventCallback (TouchEventCallback callback);

		#if UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		public static extern void EnableDefaultTouch ();

		#if UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		public static extern void DisableDefaultTouch ();

		#if UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		public static extern void EnableNativePlugin ();

		#if UNITY_ANDROID
		[DllImport ("DirectTouchEvent")]
		#endif
		public static extern void DisableNativePlugin ();

		[AOT.MonoPInvokeCallbackAttribute(typeof(TouchEventCallback))]
		static void Callback(IntPtr ptrTouchInfo, int nVal) 
		{ 
			if (nVal == 0) return;

			IntPtr ptr = ptrTouchInfo;
			List<TouchInfo> touches = new List<TouchInfo> ();
			for (var i = 0; i < nVal; i++) {
				Touch touch = (Touch)Marshal.PtrToStructure (ptr, typeof(Touch));

				TouchInfo touchInfo = new TouchInfo (touch);
				switch (touch.phase) {
				case 0:
					touchInfo.phase = TouchPhase.Began;
					break;
				case 1:
					touchInfo.phase = TouchPhase.Moved;
					break;
				case 2:
					touchInfo.phase = TouchPhase.Canceled;
					break;
				case 3:
					touchInfo.phase = TouchPhase.Ended;
					break;
				}
				touches.Add (touchInfo);

				if (i < nVal - 1) {
				ptr = (IntPtr)((int)ptr + Marshal.SizeOf (typeof(TouchInfo)));
				}
			}
		}

		void Awake() {
			if (InitializationManager () == 0) {
				RegisterTouchEventCallback (Callback);
			}
		}
	}
}