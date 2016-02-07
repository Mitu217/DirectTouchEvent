using UnityEngine;
using System.Collections.Generic;
using InputSupport;
using MobileNativeTouch;

public class MobileTouch : InputPluginBase<IMobileTouchEventHandler, MobileTouchEventHandlerManager, TouchInfo, MobileTouchInfoManager> {
	private static GameObject _gameObject;

	void Awake()
	{
		_gameObject = gameObject;
		DontDestroyOnLoad (_gameObject);
	}

	void Start ()
	{
		EnableDefaultTouch ();
	}

	public static TouchInfo[] touches { 
		get { 
			return infoManager.CurrentInfo.ToArray ();
		}
	}

	public static void EnableDefaultTouch()
	{
		foreach (var nativeTouchEvent in _gameObject.GetComponents<MobileNativeTouchEvent> ()) {
			Destroy (nativeTouchEvent);
		}

		if (_gameObject.GetComponent<DefaultTouchEvent> () == null) {
			_gameObject.AddComponent<DefaultTouchEvent> ();
		} else {
			int count = _gameObject.GetComponents<DefaultTouchEvent> ().Length;
			foreach (var defaultTouchEvent in _gameObject.GetComponents<DefaultTouchEvent> ()) {
				if (count == 1) { 
					break; 
				} else {
					count--;
				}
				Destroy (defaultTouchEvent);
			}
		}
	}

	public static void EnableMobileNativeTouch()
	{
		if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer) {
			Debug.LogWarning ("current platform isn't Mobile.");
			return;
		}
			
		foreach (var defaultTouchEvent in _gameObject.GetComponents<DefaultTouchEvent> ()) {
			Destroy (defaultTouchEvent);
		}

		if (_gameObject.GetComponent<MobileNativeTouchEvent> () == null) {
			_gameObject.AddComponent<MobileNativeTouchEvent> ();
		} else {
			int count = _gameObject.GetComponents<MobileNativeTouchEvent> ().Length;
			foreach (var nativeTouchEvent in _gameObject.GetComponents<MobileNativeTouchEvent> ()) {
				if (count == 1) { 
					break; 
				} else {
					count--;
				}
				Destroy (nativeTouchEvent);
			}
		}
	}

	public static void EnableUnityDefaultTouch() 
	{ 
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			if (_gameObject.GetComponent<MobileNativeTouchEvent> () != null) {
				_gameObject.GetComponent<MobileNativeTouchEvent> ().EnableDefaultTouchEvent ();
			}
		}
	}

	public static void DisableUnityDefaultTouch() 
	{ 
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			if (_gameObject.GetComponent<MobileNativeTouchEvent> () != null) {
				_gameObject.GetComponent<MobileNativeTouchEvent> ().DisableDefaultTouchEvent ();
			}
		}
	}
}
