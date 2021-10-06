using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerMovement1 : MonoBehaviour {

    Rigidbody rigidBody;
    
    //MOVEMENT
	public float walkSpeed;
	public float jumpSpeed;

    //ROTATION
    RaycastHit hit;
	Ray camRay;
	float rayDistance=1000.0f;
	LayerMask floorMask;


	void Start () {
        rigidBody = GetComponent<Rigidbody>();
		floorMask = LayerMask.GetMask("Floor");
	}
	

	void Update () {
        
	}

	void FixedUpdate(){
		Rotate ();
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rigidBody.AddForce(CalcVelocityChange(inputVector), ForceMode.VelocityChange);
    }

	Vector3 CalcVelocityChange(Vector3 inputVector)
	{
        Vector3 relativeVelocity = transform.TransformDirection(inputVector);
        relativeVelocity.z *= walkSpeed;
        relativeVelocity.x *= walkSpeed;
        Vector3 currRelativeVelocity = rigidBody.velocity;
        Vector3 velocityChange = relativeVelocity - currRelativeVelocity;
        velocityChange.y = 0;

        return velocityChange;
	}

	void Rotate()
	{
		camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Physics.Raycast(camRay,out hit,rayDistance,floorMask))
		{
			Vector3 playerToMouse=hit.point-transform.position;
			Quaternion rotation = Quaternion.LookRotation(playerToMouse);
			rotation.z=0; rotation.x=0;
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100 * Time.deltaTime);
		}
	}
	
}
