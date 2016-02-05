using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class DirectTouchEvent : MonoBehaviour {

	/***********************************
	 * Structure for Shared Native C++
	 **********************************/
	[StructLayout(LayoutKind.Sequential)]
	public struct TouchInfo {
		public int touchID;
		public double timestamp;
		public int phase;
		public float posX;
		public float posY;
		public float radius;
		public float pressure;
	};
		
	/****************
	 * Inner Params
	 ****************/
	private List<IDirectTouchEventHandler> registedHandlers;
	public List<DirectTouchInfo> currentTouchInfo;


	public DirectTouchEvent() 
	{
		currentTouchInfo = new List<DirectTouchInfo> ();
		registedHandlers = new List<IDirectTouchEventHandler> ();
	}

	void Update() 
	{
		/*
		for (var i = currentTouchInfo.Count - 1; i >= 0; i--) {
			if (currentTouchInfo [i].phase == TouchPhase.Began) {
				currentTouchInfo [i].phase = TouchPhase.Moved;
			}
			if (currentTouchInfo [i].phase == TouchPhase.Ended) {
				currentTouchInfo.RemoveAt (i);
			}
		}
		*/
	}


	/*********************************
	 * Called Methods by DirectTouch
	 *********************************/
	public bool AddEventHandler(IDirectTouchEventHandler handler) 
	{
		var beforeSize = registedHandlers.Count;
		var index = registedHandlers.FindIndex (x => x.Order < handler.Order);
		if (index < 0) { index = 0; }
		registedHandlers.Insert (index, handler);
		return (registedHandlers.Count > beforeSize);
	}

	public bool RemoveEventHandler(IDirectTouchEventHandler handler) 
	{  
		var beforeSize = registedHandlers.Count;
		registedHandlers.Remove (handler);
		return (registedHandlers.Count < beforeSize);  
	}

	public bool RemoveAllEventHandler() 
	{
		registedHandlers.Clear ();
		return (registedHandlers.Count == 0);
	}

	/*******************
	 * Callback Methods
	 *******************/
	// DefaulTouchEvent
	public void DefaultCallback(List<DirectTouchInfo> info) {
		if (info == null) return;
		FireEvent (info);
	}
		
	//DirectTouchNativeBridge
	public void NativeCallback(IntPtr ptrTouchInfo, int nVal) {
		if (nVal == 0) return;

		IntPtr ptr = ptrTouchInfo;
		List<DirectTouchInfo> touchInfo = new List<DirectTouchInfo> ();
		for (var i = 0; i < nVal; i++) {
			TouchInfo touch = (TouchInfo)Marshal.PtrToStructure (ptr, typeof(TouchInfo));
		
			DirectTouchInfo info = new DirectTouchInfo (touch);
			switch (touch.phase) {
			case 0:
				info.phase = TouchPhase.Began;
				break;
			case 1:
				info.phase = TouchPhase.Moved;
				break;
			case 2:
				info.phase = TouchPhase.Canceled;
				break;
			case 3:
				info.phase = TouchPhase.Ended;
				break;
			}
			touchInfo.Add (info);

			if (i < nVal - 1) {
				ptr = (IntPtr)((int)ptr + Marshal.SizeOf (typeof(TouchInfo)));
			}
		}

		FireEvent (touchInfo);
	}
		
	private void FireEvent(List<DirectTouchInfo> touchinfo) 
	{
		// Fire TouchEvent
		FireEndedEvent(touchinfo.FindAll(i => (i.phase == TouchPhase.Ended || i.phase == TouchPhase.Canceled)));
		FireBeganEvent(touchinfo.FindAll(i => i.phase == TouchPhase.Began));
		FireMovedEvent(touchinfo.FindAll(i => i.phase == TouchPhase.Moved));
	}

	private void FireBeganEvent(List<DirectTouchInfo> touchInfo) 
	{
		if (touchInfo.Count == 0) return;

		currentTouchInfo.AddRange (touchInfo);
		foreach (var handler in registedHandlers) {
			if (handler.OnTouchEventBegan (touchInfo.ToArray())) {
				return;
			}
		}
	}

	private void FireEndedEvent(List<DirectTouchInfo> touchInfo) 
	{
		if (touchInfo.Count == 0) return;

		foreach (var info in touchInfo) {
			for (var i = currentTouchInfo.Count-1; i >= 0; i--) {
				info.deltaTime = info.eventTime - currentTouchInfo [i].eventTime;
				info.deltaDistance = info.currentScreenPosition - currentTouchInfo [i].currentScreenPosition;
				currentTouchInfo.Remove (currentTouchInfo[i]);
			}
		}

		foreach (var handler in registedHandlers) {
			if (handler.OnTouchEventEnded (touchInfo.ToArray())) {
				return;
			}
		}
	}

	private void FireMovedEvent(List<DirectTouchInfo> touchInfo)
	{
		if (touchInfo.Count == 0) return;
		
		foreach (var info in touchInfo) {
			for (var i = currentTouchInfo.Count - 1; i >= 0; i--) {
				if (info.touchId == currentTouchInfo [i].touchId) {
					info.deltaTime = info.eventTime - currentTouchInfo [i].eventTime;
					info.deltaDistance = info.currentScreenPosition - currentTouchInfo [i].currentScreenPosition;
					currentTouchInfo.RemoveAt (i);
				}
			}
			currentTouchInfo.Add (info);
		}

		// Fire MovedTouchEvent
		foreach (var handler in registedHandlers) {
			if (handler.OnTouchEventMoved (touchInfo.ToArray())) {
				return;
			}
		}
	}
}
