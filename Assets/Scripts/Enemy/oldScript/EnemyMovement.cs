using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{

    [SerializeField]
    Transform target;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float rotSpeed;
    [SerializeField]
    float rangeMax; //inizia a seguirti
    [SerializeField]
    float rangeMin; //smette di avvicinarsi
    [SerializeField]
    float rangeObstacle; //schiva oggetto

    //	bool isThereAnything;
    //	RaycastHit hit;
    //	Vector3 rightRayOrigin;
    //	Vector3 leftRayOrigin;
    //	Vector3 bottomRayOrigin;
    //	Vector3 dir;

    Rigidbody myRigidbody;
    float distance; //distanza dal player

    //NavMeshAgent agent;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //dir=(target.position-transform.position).normalized;
        //agent=GetComponent<NavMeshAgent>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, target.position);
        if (distance < rangeMax)
        {
            RotateToTarget();
            if (distance > rangeMin)
            {
                Move();
                //agent.SetDestination(target.position);
            }
            else
            {
                myRigidbody.velocity = new Vector3(0, 0, 0);
            }
            //			LookForObstacles();
            //			if(isThereAnything)
            //			{
            //				Quaternion rot=Quaternion.LookRotation(dir);
            //				transform.rotation=Quaternion.Slerp(transform.rotation,rot,Time.deltaTime);
            //			}
            //			else 
            //			{
            //				RotateToTarget();
            //			}
        }
        else
        {
            myRigidbody.velocity = new Vector3(0, 0, 0);
        }
    }

    void RotateToTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        rotation.x = 0; rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }

    void Move()
    {
        Vector3 movement = transform.forward * moveSpeed;
        myRigidbody.velocity = movement;
    }

    //	void LookForObstacles()
    //	{
    //		rightRayOrigin=transform.right;
    //		leftRayOrigin=-transform.right;
    //		Ray rightRay=new Ray(transform.position+rightRayOrigin,transform.forward*rangeObstacle);
    //		Ray leftRay=new Ray(transform.position+leftRayOrigin,transform.forward*rangeObstacle);
    //		bottomRayOrigin=-transform.forward;
    //		Ray bottomRightRay=new Ray(transform.position+bottomRayOrigin,transform.right*rangeObstacle);
    //		Ray bottomLeftRay=new Ray(transform.position+bottomRayOrigin,-transform.right*rangeObstacle);
    //
    //		Debug.DrawRay(transform.position+rightRayOrigin,transform.forward*rangeObstacle,Color.yellow);
    //		Debug.DrawRay(transform.position+leftRayOrigin,transform.forward*rangeObstacle,Color.yellow);
    //		Debug.DrawRay(transform.position+bottomRayOrigin,transform.right*3);
    //		Debug.DrawRay(transform.position+bottomRayOrigin,-transform.right*3);
    //
    //		if(Physics.Raycast(rightRay,out hit,rangeObstacle))
    //		{
    //			isThereAnything=true;
    //			dir+=hit.normal*rotSpeed;
    //		}
    //		else if(Physics.Raycast(leftRay,out hit,rangeObstacle))
    //		{
    //			isThereAnything=true;
    //			dir+=hit.normal*rotSpeed;
    //		}
    //
    //		if(Physics.Raycast (bottomRightRay, out hit, 3) || Physics.Raycast (bottomLeftRay, out hit, 3))
    //		{
    //			isThereAnything=false;
    //		}
    //	}
}
