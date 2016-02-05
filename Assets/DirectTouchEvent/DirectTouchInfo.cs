using UnityEngine;
using System.Collections;

public class DirectTouchInfo {
	public DirectTouchInfo(int id, Vector3 scrPos, float timestamp, float radius, float pressure) {
		this.touchId = id;
		this.currentScreenPosition = scrPos;
		this.deltaDistance = Vector3.zero;
		this.eventTime = timestamp;
		this.deltaTime = 0.0f;
		this.radius = radius;
		this.pressure = pressure;
	}

	public DirectTouchInfo(DirectTouchEvent.TouchInfo touchInfo) {
		this.touchId = touchInfo.touchID;
		this.currentScreenPosition = new Vector3 (touchInfo.posX, Screen.height - touchInfo.posY, 0f);
		this.deltaDistance = Vector3.zero;
		this.eventTime = (float)touchInfo.timestamp;
		this.deltaTime = 0.0f;
		this.radius = touchInfo.radius;
		this.pressure = touchInfo.pressure;
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