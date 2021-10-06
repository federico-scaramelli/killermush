using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class CyborgAI : StateBehaviour
{
    Transform target;
    public float damage;
    public float moveSpeed;
    public float rotSpeed;
    public float rangeMin;
    public float rangeMax;
    public float hitForce;
    private float attackTimer;
    public float waitTimer;
    private float timer;
    Rigidbody myRigidbody;
    Health myHealth;
    private float distance; //distanza dal player
    public LineRenderer laserLine;
    public Transform laserPoint;

    private WeaponManager manager;

    public enum States
    {
        Init,
        Approach,
        Aim,
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
        Debug.Log("INIT");
        myRigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        myHealth = transform.GetComponent<Health>();
        manager = transform.GetComponent<WeaponManager>();
        laserLine.enabled = false;
        //manager.ChangeActiveWeapon(manager.startWeapon);
        ChangeState(States.Approach);
    }

    void Approach_Enter()
    {
        Debug.Log("APPROACH");
        laserLine.enabled = false;
    }

    void Approach_FixedUpdate()
    {
        Move(moveSpeed);
        RotateToTarget(rotSpeed);
        distance = Vector3.Distance(transform.position, target.position);
        if (distance < rangeMax)
            ChangeState(States.Aim); 
        if (myHealth.currentHealth <= 0)
            ChangeState(States.Die);
    }

    void Aim_Enter()
    {
        Debug.Log("AIM");
        laserLine.enabled = true;
    }

    void Aim_Update()
    {
        laserLine.material = new Material(Shader.Find("Particles/Additive"));
        laserLine.SetColors(Color.blue, Color.blue);
        laserLine.SetPosition(0, laserPoint.position);
        laserLine.SetPosition(1, target.position);

        distance = Vector3.Distance(transform.position, target.position);
        if (distance <= rangeMin) 
            ChangeState(States.Attack);
        else
        {
            Move(moveSpeed);
            RotateToTarget(rotSpeed);
        }
        if(distance > rangeMax)
        {
            ChangeState(States.Approach);
        }
    }

    void Attack_Enter()
    {
        Debug.Log("ATTACK");
    }

    void Attack_Update()
    {
        laserLine.enabled = false;

        manager.currentWeapon.timer += Time.deltaTime;
        if (manager.currentWeapon.firstShot)
        {
            manager.currentWeapon.timer = 0;
            manager.currentWeapon.Shoot();

            manager.currentWeapon.firstShot = false;
        }
        else
        {
            if (manager.currentWeapon.timer >= manager.currentWeapon.msBetweenBullets && Time.timeScale != 0)
            {
                manager.currentWeapon.timer = 0;
                manager.currentWeapon.Shoot();
            }
        }
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
