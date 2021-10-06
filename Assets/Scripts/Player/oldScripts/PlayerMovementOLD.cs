using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerMovementOLD : MonoBehaviour {

    CharacterController _controller;

	//MOVEMENT
	[SerializeField]
	float _moveSpeed=5.0f;
	[SerializeField]
	float _jumpSpeed=10.0f;
	[SerializeField]
	float _gravity=1.0f;
	float _yVelocity=0.0f;

	//ROTATION
	RaycastHit _hit;
	Ray _camRay;
	float _rayDistance=1000.0f;
	LayerMask _floorMask;


	void Start () {
        _controller = GetComponent<CharacterController>();
		_floorMask = LayerMask.GetMask("Floor");
	}
	

	void Update () {
		Move ();
	}

	void FixedUpdate(){
		Rotate ();
	}

	void Move()
	{
		Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
		Vector3 velocity = direction*_moveSpeed;
		if(_controller.isGrounded)
		{
			if(Input.GetButtonDown("Jump"))
			{
				_yVelocity=_jumpSpeed;
			}
		}
		else
		{
			_yVelocity-=_gravity;
		}
		velocity.y=_yVelocity;
		_controller.Move(velocity*Time.deltaTime);
	}

	void Rotate()
	{
		_camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Physics.Raycast(_camRay,out _hit,_rayDistance,_floorMask))
		{
			Vector3 playerToMouse=_hit.point-transform.position;
			Quaternion rotation = Quaternion.LookRotation(playerToMouse);
			rotation.z=0; rotation.x=0;
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100 * Time.deltaTime);
		}
	}
	
}
