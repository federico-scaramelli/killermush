using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
public class BabyRhynoAI : StateBehaviour
{
    public float damage;
    public int points;
    public float moveSpeed;
    public float attackSpeed;
    public float restSpeed;
    public float rotSpeed;
    public float range;
    public float hitForce;
    public float attackTimer;
    public float restTimer;
    public float updatePathTimer;
    private float pathTimer;
    private float timer;
    Rigidbody myRigidbody;
    private float distance;
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform target;

    enum States
    {
        Init,
        Approach,
        Attack,
        Rest,
        Die
    }

	void Awake ()
    {
        Initialize<States>();
        ChangeState(States.Init);
    }

    void Init_Enter()
    {
        myRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;
        if (GameObject.FindGameObjectWithTag("Player"))
            target = GameObject.FindGameObjectWithTag("Player").transform;
        else
            target = transform;
        ChangeState(States.Approach);
    }

    //Si avvicina al player con velocità moveSpeed media. Se è abbastanza vicino attacca.
    void Approach_Enter()
    {
        pathTimer = updatePathTimer;
        agent.speed = moveSpeed;
        agent.enabled = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Approach_FixedUpdate()
    {
        RaycastHit hit;

        pathTimer -= Time.deltaTime;
        if (pathTimer <= 0)
        {
            agent.SetDestination(target.position);
            pathTimer = updatePathTimer;
        }

        distance = Vector3.Distance(transform.position, target.position);
        if (distance < range)
            if (Physics.Raycast(transform.position, transform.forward, out hit, range + 1)) 
                if(hit.transform== target.transform)
                    ChangeState(States.Attack);
    }

    //Scatta verso il player con velocità attackSpeed più alta. Se tocca qualcosa gli fa danno e si applica la fisica. Passa poi in riposo.
    void Attack_Enter()
    {
        timer = attackTimer;
        RotateToTarget(rotSpeed);
    }

    void Attack_Update()
    {
        timer -= Time.deltaTime;

        agent.enabled = false;
        agent.updatePosition = false;
        agent.updateRotation = false;

        if (timer <= 0)
            ChangeState(States.Rest);

        Move(attackSpeed);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1))
        {
            Health health = hit.transform.GetComponent<Health>();
            if (hit.transform == target.transform)
            {
                health.Damage(damage);
            }
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForceAtPosition(transform.forward * hitForce, hit.point, ForceMode.Impulse);
            }
            myRigidbody.AddForce(-transform.forward * hitForce, ForceMode.Impulse);
            ChangeState(States.Rest);
        }
    }

    void Rest_Enter()
    {
        pathTimer = updatePathTimer;
        timer = restTimer;
        agent.speed = restSpeed;
        agent.enabled = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Rest_Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
            ChangeState(States.Approach);

        pathTimer -= Time.deltaTime;
        if (pathTimer <= 0)
        {
            agent.SetDestination(target.position);
            pathTimer = updatePathTimer;
        }
    }

    void OnDestroy()
    {
         GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerPoints>().AddPoints(points);
    }

    void RotateToTarget(float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        rotation.x = 0; rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }

    void Move(float speed)
    {
        Vector3 movement = transform.forward * speed;
        myRigidbody.AddForce(movement / Time.deltaTime);
    }

    void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
