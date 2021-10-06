using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class EnemyFalciatore : MonoBehaviour {
	
	public Transform target; 
	public float moveSpeed=4;
	public float rotSpeed=100;
    public float rotSpeedHead = 25;
	public float rotMultiplier=20;
	public float damage=3000;

    [SerializeField]
    float rangeMax=23; //inizia a seguirti
    [SerializeField]
    float rangeMin=2; //smette di avvicinarsi

    float distance; //distanza dal player

    [SerializeField]
    float maxRotSpeed=15;

    Rigidbody myRigidbody;
	Transform head;

	void Start () 
	{
		myRigidbody=GetComponent<Rigidbody>();
		target=GameObject.FindGameObjectWithTag("Player").transform;
		head=transform.GetChild(0).transform;
	}

	void FixedUpdate () 
	{
        distance = Vector3.Distance(transform.position, target.position);
        if (distance < rangeMax)
        {
            myRigidbody.freezeRotation = false;
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            RotateHead();
            if (distance > rangeMin)
            {
                Attack();
                Move();
            }
            else
            {
                StopMoving();
                Attack();
            }
        }
        else
        {
            StopMoving();
            StopAttack();
        }
	}

	void RotateHead()
	{
		Quaternion rotation=Quaternion.LookRotation(target.position-transform.position);
		rotation.x=0; rotation.z=0;
		head.rotation=Quaternion.Slerp(head.rotation,rotation,Time.deltaTime*rotSpeedHead);
	}

	void Attack()
	{
        myRigidbody.maxAngularVelocity = maxRotSpeed;
        myRigidbody.AddTorque(new Vector3(0, rotMultiplier * rotSpeed * Time.deltaTime, 0));
    }
	
	void Move()
	{
		Vector3 movement = head.forward * moveSpeed;
		myRigidbody.velocity=movement;
	}

	void StopMoving()
	{
		myRigidbody.velocity=new Vector3(0,0,0);
	}

    void StopAttack()
    {
        myRigidbody.maxAngularVelocity = 1;
    }
}
