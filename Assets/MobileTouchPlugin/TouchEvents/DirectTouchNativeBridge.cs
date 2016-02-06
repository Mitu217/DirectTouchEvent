using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class DirectTouchNativeBridge {
	/*
	private delegate void TouchEventCallback(IntPtr ptrTouchInfo, int nVal);

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
	public static extern void EnableDefaultTouch ();

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#elif UNITY_ANDROID
	[DllImport ("DirectTouchEvent")]
	#endif
	public static extern void DisableDefaultTouch ();

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#elif UNITY_ANDROID
	[DllImport ("DirectTouchEvent")]
	#endif
	public static extern void EnableNativePlugin ();

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#elif UNITY_ANDROID
	[DllImport ("DirectTouchEvent")]
	#endif
	public static extern void DisableNativePlugin ();


	[AOT.MonoPInvokeCallbackAttribute(typeof(TouchEventCallback))]
	static void Callback(IntPtr ptrTouchInfo, int nVal) { DirectTouch.TouchEvent.NativeCallback (ptrTouchInfo, nVal); }

	public static bool Initialization() { return (InitializationManager () == 0) ? (RegisterTouchEventCallback (Callback) == 0) : false; }
	*/
}
