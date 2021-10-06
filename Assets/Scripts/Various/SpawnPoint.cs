using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public Transform objectToSpawn;
    public int amount=1;
    public float timer;

	void Start ()
    {
        Invoke("Spawn",0);
        InvokeRepeating("Spawn", timer, timer);
	}
	
	void Spawn()
    {
        for(int i=0;i<amount;i++)
            Instantiate(objectToSpawn, transform.position, transform.rotation);
	}
}
