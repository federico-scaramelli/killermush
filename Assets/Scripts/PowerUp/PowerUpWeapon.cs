using UnityEngine;
using System.Collections;

public class PowerUpWeapon : MonoBehaviour 
{		
	public Weapon newWeapon;
    public float timer;
    private bool timerStarted = false;

	GameObject player;
	WeaponManager manager;

	void Awake () {
		player=GameObject.FindGameObjectWithTag("Player");
		manager=player.GetComponent<WeaponManager>();
	}

	void Update(){
		transform.Rotate(0,70*Time.deltaTime,0);
        if(timerStarted)
        {
            timer -= Time.deltaTime;
            if (manager.currentWeapon.weaponName != manager.startWeapon.weaponName && manager.currentWeapon.weaponName != newWeapon.weaponName)
            {
                Destroy(gameObject);
            }
        }
            
        if (timer <= 0)
        {
            manager.ChangeWeapon(manager.startWeapon);
            Destroy(gameObject);
        }
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.name=="Player")
		{
            GameObject[] PUs;
            PUs = GameObject.FindGameObjectsWithTag("WeaponPowerUp");
            foreach (GameObject PU in PUs)
                if (!PU.transform.GetComponent<Collider>().enabled)
                    Destroy(PU);

            manager.ChangeWeapon(newWeapon);
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            timerStarted = true;
            GetComponent<DestroyAfterTime>().enabled = false; 
		}
	}
}
