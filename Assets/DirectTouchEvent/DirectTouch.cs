using UnityEngine;
using System.Collections.Generic;

public class DirectTouch {
	private static DirectTouchEvent touchEvent;
	public static DirectTouchEvent TouchEvent {
		get {
			if (touchEvent == null) {
				touchEvent = (DirectTouchEvent)GameObject.FindObjectOfType (typeof(DirectTouchEvent));
				if (touchEvent == null) {
					touchEvent = new GameObject("DirectTouchEventSystem").AddComponent<DirectTouchEvent>();
					Initialization ();
				}
			}
			return touchEvent;
		}
	}
		
	/**************************
	 * Initialize Methods
	 **************************/
	private static void Initialization() {
		EnableDefaultPlugin ();
	}

	/**************************
	 * DirectTouch Methods
	 **************************/
	// Get Current Touch Info
	public static DirectTouchInfo[] touches { 
		get { 
			return TouchEvent.currentTouchInfo.ToArray();
		}
	}

	// Registter or Unregister Eventhander
	public static bool RegisterEventHandler (IDirectTouchEventHandler handler) 
	{ 
		return (TouchEvent == null) ? false : TouchEvent.AddEventHandler (handler); 
	}
	public static bool UnregisterEventHandler (IDirectTouchEventHandler handler) 
	{ 
		return (TouchEvent == null) ? false : TouchEvent.RemoveEventHandler (handler); 
	}

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


	/**************************
	 * Inner Methods
	 **************************/
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
}
