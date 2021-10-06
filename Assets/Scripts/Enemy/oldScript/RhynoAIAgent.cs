using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class RhynoAIAgent : StateBehaviour
{
    public float damage;
    public float moveSpeed;
    public float attackSpeed;
    public float rotSpeed;
    public float range;
    public float hitForce;
    public float prepareAttackTimer;
    private float attackTimer;
    public float waitTimer;
    private float timer;
    Rigidbody myRigidbody;
    private Health myHealth;
    private float distance; //distanza dal player
    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;

    public enum States
    {
        Init,
        Approach,
        PrepareAttack,
        Attack,
        Wait,
        Die
    }

    void Awake()
    {
        Initialize<States>();
        ChangeState(States.Init);
    }

    void Init_Enter()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        myHealth = transform.GetComponent<Health>();
        myRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.stoppingDistance = range;
        agent.speed = moveSpeed;
        ChangeState(States.Approach);
    }

    void Approach_FixedUpdate()
    {
        agent.SetDestination(target.position);
        distance = Vector3.Distance(transform.position, target.position);
        if (distance < range)
            ChangeState(States.PrepareAttack);
        if (transform.GetComponent<Health>().currentHealth <= 0)
            ChangeState(States.Die);
    }

    void PrepareAttack_Enter()
    {
        timer = prepareAttackTimer;
    }

    void PrepareAttack_Update()
    {
        RotateToTarget(rotSpeed);
        //Play animation
        timer -= Time.deltaTime;
        if (timer <= 0)
            ChangeState(States.Attack);
        if (transform.GetComponent<Health>().currentHealth <= 0)
            ChangeState(States.Die);
    }

    void Attack_Enter()
    {
        timer = range/10 / attackSpeed;
        agent.enabled = false;
        myHealth.damageable = false;
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    void Attack_FixedUpdate()
    {
        timer -= Time.deltaTime;
        Move(attackSpeed);
        RotateToTarget(rotSpeed/2);
        if (timer <= 0)
        {
            ChangeState(States.Wait);
        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1))
        {
            Health health = hit.transform.GetComponent<Health>();
            if (health)
            {
                health.Damage(damage);
                Debug.Log(health.currentHealth);
            }
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForceAtPosition(transform.forward * hitForce, hit.point, ForceMode.Impulse);
            }
            myRigidbody.AddForce(-transform.forward * hitForce, ForceMode.Impulse);
            ChangeState(States.Wait);
        }
    }

    void Wait_Enter()
    {
        timer = waitTimer;
        myHealth.damageable = true;
        agent.enabled = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Wait_Update()
    {
        //Animation
        timer -= Time.deltaTime;
        distance = Vector3.Distance(transform.position, target.position);
        if (timer <= 0)
            if (distance < range)
                ChangeState(States.PrepareAttack);
            else
                ChangeState(States.Approach);
        if (transform.GetComponent<Health>().currentHealth <= 0)
            ChangeState(States.Die);
    }

    void Die_Enter()
    {
        GetComponent<DropPowerUp>().Drop();
        Destroy(gameObject);
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
}
