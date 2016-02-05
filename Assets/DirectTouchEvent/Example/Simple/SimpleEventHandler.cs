using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SimpleEventHandler : MonoBehaviour, IDirectTouchEventHandler {
	[SerializeField]	private GameObject canvas;
	[SerializeField]	private GameObject prefabGesturePoint;

	[SerializeField]	private float minScale;
	[SerializeField]	private float maxScale;

	private int currentTouchCount;
	private List<GameObject> gesturePoints;

	void Awake()
	{
		Application.targetFrameRate = 60;

		currentTouchCount = 0;
		gesturePoints = new List<GameObject> ();
	}
		
	void Start() 
	{
		foreach (Image obj in canvas.GetComponentsInChildren<Image>()) {
			gesturePoints.Add (obj.gameObject);
			obj.gameObject.SetActive (false);
		}

		// Regist EventHandler
		DirectTouch.RegisterEventHandler (this);
		DirectTouch.DisableUnityDefaultTouch ();
		//DirectTouch.DisableNativePlugin ();
	}

	void Update()
	{
		Debug.Log ("Default: " + Input.touches.Length);
	}

	#region IDirectTouchEventHandler implementation
	public bool OnTouchEventBegan (DirectTouchInfo[] touchInfo)
	{
		foreach (var info in touchInfo) {
			gesturePoints [currentTouchCount].SetActive (true);
			gesturePoints [currentTouchCount].transform.position = info.currentScreenPosition;
			currentTouchCount++;
		}

		return false; // true: break EventHandlerLoop, false: next EventHandler
	}

	public bool OnTouchEventEnded (DirectTouchInfo[] touchInfo)
	{
		foreach (var info in touchInfo) {
			currentTouchCount--;
			gesturePoints [currentTouchCount].SetActive (false);
		}
		
		return false; // true: break EventHandlerLoop, false: next EventHandler
	}

	public bool OnTouchEventMoved (DirectTouchInfo[] touchInfo)
	{
		for (int i = 0; i < touchInfo.Length; i++) {
			gesturePoints [i].transform.position = touchInfo [i].currentScreenPosition;
		}

		return false; // true: break EventHandlerLoop, false: next EventHandler
	}

	public int Order {
		get {
			return 0;
		}
	}

	public bool Process {
		get {
			return true; // true: process EventHandler, false; next EventHandler
		}
	}
	#endregion
}
