using UnityEngine;
using System.Collections;

public class MelmosoDrop : MonoBehaviour {
    
    public float damageRange;
    public float damage;
    public float msBetweenDamage;

    private float timer=0;
    private Transform player;
    private Vector3 distanceVect;
    private float distance;
    private Health health;

	void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = player.GetComponent<Health>();
	}
	
	void Update ()
    {
        distanceVect = player.position - transform.position;
        distance = distanceVect.magnitude;
        if(distance<damageRange)
        {
            timer -= Time.deltaTime;
            if(timer<=0)
            {
                health.Damage(damage);
                timer = msBetweenDamage/1000;
            }
        }else{
            timer = msBetweenDamage/1000;
        }
	}
}
