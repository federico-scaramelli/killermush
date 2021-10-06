using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	Transform target;
	[SerializeField]
	float smoothing=5.0f;

	Vector3 offset;

	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        offset =transform.position-target.position;
	}

	void Update () {
		Vector3 _newCamPos=target.position+offset;
		transform.position=Vector3.Lerp(transform.position, _newCamPos, smoothing*Time.deltaTime);
	}
}
