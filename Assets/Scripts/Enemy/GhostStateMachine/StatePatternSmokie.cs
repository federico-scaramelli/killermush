using UnityEngine;
using System.Collections;

public class StatePatternSmokie : MonoBehaviour
{
    public float damage;
    public float meleeDamage;
    public float hitForce;
    public float moveSpeed;
    public float attackSpeed;
    public float rotSpeed;
    public float range;
    public int points;
    public int addedToRange = 3;

    //TO HIDE IN INSPECTOR OR SET PRIVATE
    public bool canAttack=true;
    public bool collideWithPlayer = false;
    public int clickValue;
    public TextMesh text;

    public KeyCode key;
    public float timeBetweenAttacks;
    public float clickValueSubSpeed;
    public float updatePathTimer;
    [HideInInspector] public float pathTimer;
    [HideInInspector] public float timer;
    [HideInInspector] public Rigidbody myRigidbody;
    [HideInInspector] public Health myHealth;
    [HideInInspector] public Health playerHealth;
    [HideInInspector] public float distance;
    [HideInInspector] public Transform target;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;

    public ISmokieState currentState;
    public RestStateSmokie restState;
    public ApproachStateSmokie approachState;
    public MeleeAtkStateSmokie meleeAttackState;
    public AttackStateSmokie attackState;

    void Awake()
    {
        restState = new RestStateSmokie(this);
        approachState = new ApproachStateSmokie(this);
        meleeAttackState = new MeleeAtkStateSmokie(this);
        attackState = new AttackStateSmokie(this);

        if (GameObject.FindGameObjectWithTag("Player"))
            target = GameObject.FindGameObjectWithTag("Player").transform;
        else
            target = transform;
        myHealth = transform.GetComponent<Health>();
        myRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.stoppingDistance = range;
        agent.speed = moveSpeed;
        playerHealth = target.transform.GetComponent<Health>();
    }

    void Start ()
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

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.transform.name != "Floor")
        {
            if (currentState == meleeAttackState)
            {
                //Debug.Log(col.collider.name);
                if (col.collider.transform.GetComponent<Rigidbody>())
                {
                    col.collider.transform.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * hitForce, col.collider.transform.position, ForceMode.Impulse);
                }
                Health health = col.collider.transform.GetComponent<Health>();
                if (health && col.transform==target.transform)
                {
                    health.Damage(meleeDamage);
                }
                if (col.collider.transform.tag == "Player")
                {
                    collideWithPlayer = true;
                }
            }
        }
    }

    void OnCollisionExit()
    {
        collideWithPlayer = false;
    }

   
    private bool flag=false;

    public void substractValueFunction()
    {
        if (flag == false)
        {
            if (clickValue > 0)
            {
                StartCoroutine("substractValue", clickValueSubSpeed);
                flag = true;
            }
        }
        else
        {
            if(clickValue<=0)
            {
                clickValue = 0;
                StopCoroutine(substractValue(clickValueSubSpeed));
                flag = false;
            }
        }
    }

    IEnumerator substractValue(float waitForSeconds)
    {
        while(true)
        {
            clickValue--;
            yield return new WaitForSeconds(waitForSeconds);
        }
    }
}
