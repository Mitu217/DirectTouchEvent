using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InputSupport
{
	public abstract class InputEventHandlerManager<EventHandler, InputInfo> 
		where EventHandler : IInputEventHandler 
	{
		public abstract void FireEvent (List<InputInfo> touches);

		/****************
		 * Inner Params
		 ****************/
		protected List<EventHandler> registedHandlers;


		/**************
		 * Constructor
		 **************/
		public InputEventHandlerManager() 
		{
			registedHandlers = new List<EventHandler> ();
		}

		/*************************************
		 * Register & Unregister EventHandler
		 *************************************/
		public bool RegisterEventHandler(EventHandler handler) 
		{
			List<EventHandler> _registedHandlers = new List<EventHandler> (registedHandlers);

			var index = registedHandlers.FindIndex (x => x.Order < handler.Order);
			if (index < 0) { index = 0; }
			registedHandlers.Insert (index, handler);

			if (registedHandlers.Count <= _registedHandlers.Count) {
				Debug.LogWarning ("[Register Error] InfoManager");
				registedHandlers = new List<EventHandler> (_registedHandlers);
				return false;
			}

			return true;
		}

		public bool RegisteAllEventHandler(IEnumerable<EventHandler> handlers)
		{
			List<EventHandler> _registedHandlers = new List<EventHandler> (registedHandlers);

			foreach (var handler in handlers) {
				if (!RegisterEventHandler (handler)) {
					registedHandlers = new List<EventHandler> (_registedHandlers);
					return false;
				}
			}

			return true;
		}

		public bool UnregisterEventHandler(EventHandler handler) 
		{  
			List<EventHandler> _registedHandlers = new List<EventHandler> (registedHandlers);

			registedHandlers.Remove (handler);

			if (registedHandlers.Count >= _registedHandlers.Count) {
				Debug.LogWarning ("[Unregister Error] InfoManager");
				registedHandlers = new List<EventHandler> (_registedHandlers);
				return false;
			}

			return true;
		}

		public bool UnregisterAllEventHandler(IEnumerable<EventHandler> handlers)
		{
			List<EventHandler> _registedHandlers = new List<EventHandler> (registedHandlers);

			foreach (var handler in handlers) {
				if (!UnregisterEventHandler (handler)) {
					registedHandlers = new List<EventHandler> (_registedHandlers);
					return false;
				}
			}

			return true;
		}

		public bool ClearEventHandler() 
		{
			List<EventHandler> _registedHandlers = new List<EventHandler> (registedHandlers);

			registedHandlers.Clear ();

			if (registedHandlers.Count != 0) {
				registedHandlers = new List<EventHandler> (_registedHandlers);
				return false;
			}

			return true;
		}
	}
}
