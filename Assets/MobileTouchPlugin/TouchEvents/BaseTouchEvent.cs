/****************************************
 * 
 * タッチイベントプラグインのための継承元クラス
 * 
 ****************************************/

using UnityEngine;
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

	public abstract class BaseTouchEvent : MonoBehaviour { 
		private MobileTouchInfoManager mobileTouchInfoManager;
		public MobileTouchInfoManager MobileTouchInfoManager
		{
			get {
				return mobileTouchInfoManager;
			}
			set {
				mobileTouchInfoManager = value;
			}
		}
	}
}
