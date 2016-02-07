using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InputSupport;

namespace MobileNativeTouch 
{
	public class MobileTouchInfoManager : InputInfoManager<TouchInfo> 
	{
		private List<TouchInfo> currentTouchInfo = new List<TouchInfo>();

		#region implemented abstract members of InputInfoManager
		public override List<TouchInfo> CurrentInfo {
			get {
				if (currentTouchInfo == null) {
					currentTouchInfo = new List<TouchInfo> ();
				}

				return currentTouchInfo;
			}
		}

		public override int InfoCount {
			get {
				if (currentTouchInfo != null)
					return currentTouchInfo.Count;

				currentTouchInfo = new List<TouchInfo> ();
				return currentTouchInfo.Count;
			}
		}

		public override void Add (TouchInfo info)
		{
			currentTouchInfo.Add (info);
		}

		public override void Remove (TouchInfo info)
		{
			currentTouchInfo.Remove (info);
		}

		public override void Update (TouchInfo info)
		{
			if (currentTouchInfo == null){
				currentTouchInfo = new List<TouchInfo> ();
			}

			TouchInfo targetTouchInfo = currentTouchInfo.Find (x => x.touchId == info.touchId);
			if (targetTouchInfo != null) {
				info.deltaDistance = info.currentScreenPosition - targetTouchInfo.currentScreenPosition;
				info.deltaTime = info.eventTime - targetTouchInfo.eventTime;
				Add (info);
				Remove (targetTouchInfo);
			} else {
				Clear ();
			}
		}

		public override void Clear ()
		{
			foreach(var currentInfo in currentTouchInfo) {
				currentInfo.phase = TouchPhase.Ended;
			}
		}


		#endregion
	}
}