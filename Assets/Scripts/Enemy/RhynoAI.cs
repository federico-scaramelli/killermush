using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class RhynoAI : StateBehaviour
{
    Transform target;
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
    Health myHealth;
    private float distance; //distanza dal player

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
        Debug.Log("INIZIALIZZO");
        myRigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        myHealth = transform.GetComponent<Health>();
        ChangeState(States.Approach);
    }
    
    void Approach_Enter()
    {
        Debug.Log("APPROACH");
    }

    void Approach_FixedUpdate()
    {
        Move(moveSpeed);
        RotateToTarget(rotSpeed);
        distance = Vector3.Distance(transform.position, target.position);
        if(distance<range)
            ChangeState(States.PrepareAttack);
        if (myHealth.currentHealth <= 0)
            ChangeState(States.Die);
    }

    void PrepareAttack_Enter()
    {
        timer = prepareAttackTimer;
        Debug.Log("PREPARO");
    }

    void PrepareAttack_Update()
    {
        RotateToTarget(rotSpeed);
        //Play animation
        timer -= Time.deltaTime;
        if (timer <= 0)
            ChangeState(States.Attack);
        if (myHealth.currentHealth <= 0)
            ChangeState(States.Die);
    }

    void Attack_Enter()
    {
        Debug.Log("ATTACK");
        timer = (range/10)/attackSpeed;
        myHealth.damageable = false;
    }

    void Attack_Update()
    {
        timer -= Time.deltaTime;
        Move(attackSpeed);
        //RotateToTarget(rotSpeed/5);
        if (timer<=0)
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
            myRigidbody.AddForce(-transform.forward*hitForce, ForceMode.Impulse);
            ChangeState(States.Wait);
        }
    }

    void Wait_Enter()
    {
        timer = waitTimer;
        myHealth.damageable = true;
        Debug.Log("WAIT");
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
        if (myHealth.currentHealth <= 0)
            ChangeState(States.Die);
    }

    void Die_Enter()
    {
        Debug.Log("MORTO");
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
