using UnityEngine;

public class TouchInfo 
{
	public TouchInfo(int id, Vector3 scrPos, float timestamp, float radius, float pressure) {
		this.touchId = id;
		this.currentScreenPosition = scrPos;
		this.deltaDistance = Vector3.zero;
		this.eventTime = timestamp;
		this.deltaTime = 0.0f;
		this.radius = radius;
		this.pressure = pressure;
	}

	public TouchInfo(MobileNativeTouch.Touch touch) {
		this.touchId = touch.touchID;
		this.currentScreenPosition = new Vector3 (touch.posX, Screen.height - touch.posY, 0f);
		this.deltaDistance = Vector3.zero;
		this.eventTime = (float)touch.timestamp;
		this.deltaTime = 0.0f;
		this.radius = touch.radius;
		this.pressure = touch.pressure;
	}

	public int touchId{ get; set; }

	public Vector3 currentScreenPosition{ get; set; }

	public Vector3 deltaDistance{ get; set;	}

	public float eventTime { get; set; }

	public float deltaTime{ get; set; }

	public float radius { get; set; }

	public TouchPhase phase { get; set; }

	public float pressure { get; set; }
}