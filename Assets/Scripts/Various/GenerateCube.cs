using UnityEngine;
using System.Collections;

public class GenerateCube : MonoBehaviour {

    Ray _camRay;
    RaycastHit _hit;
    float _rayDistance = 1000.0f;
    public GameObject cube;
    public int quantity;

	void FixedUpdate () {
        _camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_camRay, out _hit, _rayDistance))
        {
            if(Input.GetMouseButtonDown(1))
            {
                for(int i=0;i<quantity;i++)
                {
                    Instantiate(cube, new Vector3(_hit.point.x,_hit.point.y+0.7f*i,_hit.point.z), Quaternion.LookRotation(cube.transform.forward));
                }
            }
        }
    }
}
