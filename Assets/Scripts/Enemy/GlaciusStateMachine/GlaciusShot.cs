using UnityEngine;
using System.Collections;

public class GlaciusShot : MonoBehaviour {
    public float speedMultiplier;
    public float timer;
    private bool timerStarted = false;
    private float oldValue;
    GameObject player;
    PlayerController controller;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    void Update() 
	{
        if(timerStarted)
        {
            timer -= Time.deltaTime;
            if(timer<=0)
            {
                controller.speed = oldValue;
                controller.speedDown = false;
                Destroy(gameObject);
            }
        }
	}
	
	void OnCollisionEnter(Collision col)
	{
        Transform collider = col.collider.transform;
        if (collider == player.transform && !controller.speedDown)
        {
            oldValue = controller.speed;
            controller.speed *= speedMultiplier;
            timerStarted = true;
            controller.speedDown = true;
            transform.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            GetComponent<DestroyAfterTime>().enabled = false;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}