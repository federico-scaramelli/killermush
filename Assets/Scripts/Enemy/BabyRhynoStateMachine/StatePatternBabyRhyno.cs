using UnityEngine;
using System.Collections;

public class StatePatternBabyRhyno : MonoBehaviour
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
    public float checkIfPlayerIsForwardTimer;
    [HideInInspector] public float pathTimer;
    [HideInInspector] public float timer;
    [HideInInspector] public Rigidbody myRigidbody;
    [HideInInspector] public float distance;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;
    [HideInInspector] public Transform target;

    public IBabyRhynoState currentState;
    public RestStateBabyRhyno restState;
    public ApproachStateBabyRhyno approachState;
    public AttackStateBabyRhyno attackState;

    void Awake()
    {
        restState = new RestStateBabyRhyno(this);
        approachState = new ApproachStateBabyRhyno(this);
        attackState = new AttackStateBabyRhyno(this);

        myRigidbody = GetComponent<Rigidbody>();
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
