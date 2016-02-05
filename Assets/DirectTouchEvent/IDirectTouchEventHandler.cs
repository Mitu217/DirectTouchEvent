using UnityEngine;
using System.Collections;

public interface IDirectTouchEventHandler
{
	int Order { get; }
	bool Process { get; }

	bool OnTouchEventBegan (DirectTouchInfo[] touchInfo);
	bool OnTouchEventEnded (DirectTouchInfo[] touchInfo);
	bool OnTouchEventMoved (DirectTouchInfo[] touchInfo);
	//bool OnTouchEventStationary (DirectTouchInfo[] touchInfo);
}
