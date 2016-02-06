using UnityEngine;
using System.Collections;
using InputSupport;

public abstract class InputPluginBase<IEventHandler, EventHandlerManager, TouchInfo, InfoManager> : MonoBehaviour
	where IEventHandler : IInputEventHandler
	where EventHandlerManager : InputEventHandlerManager<IEventHandler, TouchInfo>, new() 
	where InfoManager : InputInfoManager<TouchInfo>, new() 
{
	private static EventHandlerManager eventHandlerManager;
	protected static InfoManager infoManager;

	public InputPluginBase()
	{
		eventHandlerManager = new EventHandlerManager ();
		infoManager = new InfoManager ();
	}
		
	void Update()
	{
		if (infoManager.InfoCount > 0) {
			eventHandlerManager.FireEvent (infoManager.CurrentInfo);
		}
	}

	public static bool RegisterEventHandler (IEventHandler handler) 
	{ 
		return  eventHandlerManager.RegisterEventHandler (handler);
	}

	public static bool UnregisterEventHandler (IEventHandler handler) 
	{ 
		return eventHandlerManager.UnregisterEventHandler (handler); 
	}
}
