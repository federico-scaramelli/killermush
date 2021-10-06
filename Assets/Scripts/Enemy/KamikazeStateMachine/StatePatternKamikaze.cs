using UnityEngine;
using System.Collections;

public class StatePatternKamikaze : MonoBehaviour
{
    public float damage;
    public float damageMultiplier;
    public float damageMultiplierToPlayer;
    public float explosionTimer;
    public float explosionForce;
    public float explosionForceToPlayer;
    public float explosionRadius;
    public float moveSpeed;
    public float attackSpeed;
    public float rotSpeed;
    public float rangeMin;
    public float rangeMax;
    public int points;

    [HideInInspector]
    public float timer;
    public float updatePathTimer;
    [HideInInspector]
    public float pathTimer;
    [HideInInspector]
    public Rigidbody myRigidbody;
    [HideInInspector]
    public Health myHealth;
    [HideInInspector]
    public Health playerHealth;
    [HideInInspector]
    public float distance;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent agent;
    //[HideInInspector]
    //public Animator anim;

    public IKamikazeState currentState;
    public ApproachStateKamikaze approachState;
    public AttackStateKamikaze attackState;
    public BoomStateKamikaze boomState;

    void Awake()
    {
        approachState = new ApproachStateKamikaze(this);
        attackState = new AttackStateKamikaze(this);
        boomState = new BoomStateKamikaze(this);

        if (GameObject.FindGameObjectWithTag("Player"))
            target = GameObject.FindGameObjectWithTag("Player").transform;
        else
            target = transform;
        myHealth = transform.GetComponent<Health>();
        myRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //anim = GetComponent<Animator>();
        //anim.SetFloat("Speed", move);
        agent.stoppingDistance = 1;
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;
        playerHealth = target.transform.GetComponent<Health>();
    }

    void Start()
    {
        currentState = approachState;
        currentState.EnterState();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerPoints>().AddPoints(points);
    }
}
