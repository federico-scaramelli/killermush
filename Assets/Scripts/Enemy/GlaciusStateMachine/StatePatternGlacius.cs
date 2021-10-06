using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatePatternGlacius : MonoBehaviour {
    
    public int points;
    public float moveSpeed;
    public float rotSpeed;
    public float distanceAttackRange;
    public float msBetweenShots;
    public float shootForce;
    public float shootDamage;
    public float precision;
    public Transform shot;
    public Transform shootPoint;
    public float meleeAttackRange;
    public float meleeAttackDamage;
    public float checkDamagesSeconds;
    public float maxDamages;
    public float shieldTimer;
    public float shieldHealth;
    public float shieldResistance;
    public float shieldHealthRegenPerSecond;
    public Image shieldBarImage;
    private bool timerStarted = false;
    private float startHealth;
    private float startHealthToDetectDamage;
    public float updatePathTimer;
    //public float checkIfPlayerIsForwardTimer;
    [HideInInspector] public float pathTimer;
    [HideInInspector] public float timer;
    [HideInInspector] public float timer2;
    [HideInInspector] public float timer3;
    [HideInInspector] public Rigidbody myRigidbody;
    [HideInInspector] public float distance;
    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;
    [HideInInspector] public Transform target;
    [HideInInspector] public Health myHealth;

    public IGlaciusState currentState;
    public ApproachStateGlacius approachState;
    public DistanceAttackStateGlacius distanceAttackState;
    public MeleeAttackStateGlacius meleeAttackState;
    public IceShieldStateGlacius iceShieldState;

    void Awake()
    {
        approachState = new ApproachStateGlacius(this);
        distanceAttackState = new DistanceAttackStateGlacius(this);
        meleeAttackState = new MeleeAttackStateGlacius(this);
        iceShieldState = new IceShieldStateGlacius(this);

        myHealth = GetComponent<Health>();
        myRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;
        agent.stoppingDistance = distanceAttackRange;
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
        //Debug.Log(distance);
        if(currentState!=iceShieldState)
        {
            if(timerStarted)
            {
                timer2 -= Time.deltaTime;
                if (timer2 <= 0)
                {
                    timerStarted = false;
                    startHealth = myHealth.currentHealth;
                }
                if (startHealth - myHealth.currentHealth > maxDamages)
                {
                    currentState = iceShieldState;
                    currentState.EnterState();
                }
            }
        }
        //Debug.Log(currentState.ToString());
        currentState.UpdateState();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
        if (currentState != iceShieldState)
            startHealthToDetectDamage = myHealth.currentHealth;
    }

    void LateUpdate()
    {
        if (currentState != iceShieldState)
        {
            if (myHealth.currentHealth != startHealthToDetectDamage && !timerStarted)
                StartTimer();
        }
    }

    void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
			if (GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerPoints>())
				GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerPoints>().AddPoints(points);
    }

    void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void StartTimer()
    {
        timer2 = checkDamagesSeconds;
        timerStarted = true;
        startHealth = myHealth.currentHealth;
    }
}