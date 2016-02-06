using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InputSupport;

namespace MobileNativeTouch
{
	public class MobileTouchEventHandlerManager : InputEventHandlerManager<IMobileTouchEventHandler, TouchInfo> 
	{
		#region implemented abstract members of InputEventHandlerManager
		public override void FireEvent (List<TouchInfo> touches)
		{
			FireEndedEvent(touches.FindAll(i => (i.phase == TouchPhase.Ended || i.phase == TouchPhase.Canceled)));
			FireBeganEvent(touches.FindAll(i => i.phase == TouchPhase.Began));
			FireMovedEvent(touches.FindAll(i => i.phase == TouchPhase.Moved));
		}
		#endregion

		private void FireBeganEvent(List<TouchInfo> touches) 
		{
			if (touches.Count == 0)
				return;

			foreach (var handler in registedHandlers) {
				if (handler.OnTouchEventBegan (touches.ToArray())) {
					return;
				}
			}
		}

		private void FireEndedEvent(List<TouchInfo> touches) 
		{
			if (touches.Count == 0)
				return;

			foreach (var handler in registedHandlers) {
				if (handler.OnTouchEventEnded (touches.ToArray())) {
					return;
				}
			}
		}

		private void FireMovedEvent(List<TouchInfo> touches)
		{
			if (touches.Count == 0)
				return;

			foreach (var handler in registedHandlers) {
				if (handler.OnTouchEventMoved (touches.ToArray())) {
					return;
				}
			}
		}

		private void FireStationaryEvent (List<TouchInfo> touches) 
		{
			if (touches.Count == 0)
				return;

			foreach (var handler in registedHandlers) {
				if (handler.OnTouchEventStationary(touches.ToArray())) {
					return;
				}
			}
		}
	}
}