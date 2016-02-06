using UnityEngine;
using System.Collections.Generic;
using InputSupport;
using MobileNativeTouch;

public class MobileTouch : InputPluginBase<IMobileTouchEventHandler, MobileTouchEventHandlerManager, TouchInfo, MobileTouchInfoManager> {
	private GameObject _gameObject;

	void Awake()
	{
		_gameObject = gameObject;
	}

	void Start ()
	{
		if (_gameObject.GetComponent<DefaultTouchEvent> () == null) {
			#if UNITY_ANDROID
			_gameObject.AddComponent<AndroidTouchEvent> ().MobileTouchInfoManager = infoManager;
			#else
			_gameObject.AddComponent<DefaultTouchEvent> ().MobileTouchInfoManager = infoManager;
			#endif
		} else {
			int count;
			#if UNITY_ANDROID
			count = _gameObject.GetComponents<AndroidTouchEvent> ().Length;
			foreach (var androidTouchEvent in _gameObject.GetComponents<AndroidTouchEvent> ()) {
				if (count == 1) { break; }
				Destroy (androidTouchEvent);
			}
			#else
			count = _gameObject.GetComponents<DefaultTouchEvent> ().Length;
			foreach (var defaultTouchEvent in _gameObject.GetComponents<DefaultTouchEvent> ()) {
				if (count == 1) { break; }
				Destroy (defaultTouchEvent);
			}
			#endif
		}
	}

	public static TouchInfo[] touches { 
		get { 
			return infoManager.CurrentInfo.ToArray ();
		}
	}

		
	/*
	// Enable or Disable UnityDefaultTouchEvent
	public static void EnableUnityDefaultTouch() 
	{ 
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			DirectTouchNativeBridge.EnableDefaultTouch (); 
		}
	}

	public static void DisableUnityDefaultTouch() 
	{ 
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			DirectTouchNativeBridge.DisableDefaultTouch (); 
		}
	}


	// Enable or Disable NativePlugin
	public static void EnableNativePlugin()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			DirectTouchNativeBridge.EnableNativePlugin ();
			DisableDefaultPlugin ();
		}	
	}

	public static void DisableNativePlugin()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			DirectTouchNativeBridge.DisableNativePlugin ();
		}

		EnableDefaultPlugin ();
	}
	*/

	/**************************
	 * Inner Methods
	 **************************/
	/*
	private static void EnableDefaultPlugin()
	{
		if (touchEvent != null && touchEvent.GetComponent<DefaultTouchEvent> () == null) {
			touchEvent.gameObject.AddComponent<DefaultTouchEvent> ().Initialization (touchEvent);
		}
	}

	private static void DisableDefaultPlugin()
	{
		if (touchEvent != null && touchEvent.GetComponent<DefaultTouchEvent> () != null) {
			touchEvent.GetComponent<DefaultTouchEvent> ().Finalization ();
			GameObject.Destroy (touchEvent.GetComponent<DefaultTouchEvent> ());
		}
	}
	*/
}
