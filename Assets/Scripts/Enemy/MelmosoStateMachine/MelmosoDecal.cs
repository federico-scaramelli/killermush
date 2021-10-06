using UnityEngine;
using System.Collections;

public class MelmosoDecal : MonoBehaviour
{
    public float damage;
    public float msBetweenDamage;

    private bool collisionChecked = false;
    private float timer = 0;
    private Transform player;
    private Health health;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = player.GetComponent<Health>();
    }

    void Update ()
    {
	    if(collisionChecked)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                health.Damage(damage);
                timer = msBetweenDamage / 1000;
            }
        }    
	}
	
	void OnTriggerEnter (Collider col)
    {
        if (col.transform == player.transform)
        {
            collisionChecked = true;
        }
	}

    void OnTriggerExit (Collider col)
    {
        if(col.transform==player.transform)
        {
            collisionChecked = false;
        }
    }
}
