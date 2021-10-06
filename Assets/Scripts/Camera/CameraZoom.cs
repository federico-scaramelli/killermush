using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	[SerializeField]
	float zoomSpeed;
	[SerializeField]
	float minDistance;
	[SerializeField]
	float maxDistance;

	void Update () {
		Camera.main.fieldOfView-=Input.GetAxis("Mouse ScrollWheel")*zoomSpeed;
		Camera.main.fieldOfView=Mathf.Clamp (Camera.main.fieldOfView, minDistance, maxDistance);
	}
}
