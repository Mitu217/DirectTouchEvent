using UnityEngine;
using System.Collections;
using InputSupport;

public interface IMobileTouchEventHandler : IInputEventHandler
{
	bool OnTouchEventBegan (TouchInfo[] touchInfo);
	bool OnTouchEventEnded (TouchInfo[] touchInfo);
	bool OnTouchEventMoved (TouchInfo[] touchInfo);
	bool OnTouchEventStationary (TouchInfo[] touchInfo);
}
