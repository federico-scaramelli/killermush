using UnityEngine;
using System.Collections;

public class StatePatternMelmoso : MonoBehaviour
{
    [HideInInspector] public Transform target;
    public Transform skifBall;
    public Transform shootPoint;
    public Transform suicideSmoke;
    public float moveSpeed;
    public float rotSpeed;
    public float attackRange;
    public float minDistanceToTarget;
    public float dangerRange;
    public float explDamageMultiplier;
    public float explosionForce;
    public float attackTimer;
    public float shootAngle;
    public float precision;
    public float points;
    public float updatePathTimer;
    [HideInInspector] public float pathTimer;
    [HideInInspector] public float timer;
    [HideInInspector] public float gravity;
    [HideInInspector] public float distance;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;

    public IMelmosoState currentState;
    public ApproachStateMelmoso approachState;
    public AttackStateMelmoso attackState;
    public SuicideStateMelmoso suicideState;

    void Awake()
    {
        approachState = new ApproachStateMelmoso(this);
        attackState = new AttackStateMelmoso(this);
        suicideState = new SuicideStateMelmoso(this);
        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;
        if (GameObject.FindGameObjectWithTag("Player"))
            target = GameObject.FindGameObjectWithTag("Player").transform;
        else
            target = transform;
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;
        agent.stoppingDistance = minDistanceToTarget;
        gravity = Physics.gravity.magnitude;

        currentState = approachState;
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();
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

    void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
