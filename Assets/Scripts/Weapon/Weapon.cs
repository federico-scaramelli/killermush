using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Classe che rappresenta un'arma di gioco in ogni sua componente (Grafica+Logica+Dati)
/// </summary>
[System.Serializable]
public class Weapon : MonoBehaviour
{
    [HideInInspector] public GameObject shootPoint;
    [HideInInspector] public Light faceLight;
    [HideInInspector] public ParticleSystem muzzleFlash;
    [HideInInspector] public Light gunLight;
    [HideInInspector] public LineRenderer gunLine;

    public string weaponName;
    public float effectsTime;
    public Bullet bullet;
    public bool canShot;
    public bool blockRotation;
    public bool blockMove;
    public float damage;
    public float range;
    public int pooledAmount;
    public float precision;
    public float msBetweenBullets;
    public int bulletNumber;
    public float rinculo;
    public float spinta;
    public bool drawLine;

    //Vettori per power up potenziamento arma. Il primo rappresenta i valori che i powerup assegnano alle caratteristiche della tua arma;
    public int[] precisionUpValues;
    // il secondo i flag che ne verificano l'attivazione.
    [HideInInspector] public bool[] precisionUpFlag;
    public int[] rangeUpValues;
    [HideInInspector] public bool[] rangeUpFlag;

    [HideInInspector] public List<GameObject> bullets;
    [HideInInspector] public bool firstShot=true;
    [HideInInspector] public float timer;
    Rigidbody rb;
    Transform user;
    
    public void Start()
    {
        poolObjects();

        setPUFlagsFalse();
    }

    public void Shoot()
    {
        if(canShot)
        {
            user = transform.parent;
            rb = user.transform.GetComponent<Rigidbody>();

            //Attiva effetti
            gunLight.enabled = true;
            muzzleFlash.Stop();
            muzzleFlash.Play();
            faceLight.enabled = true;
            gunLine.SetPosition(0, shootPoint.transform.position);

            //Blocco rotazione/movimento
            if (user.transform.GetComponent<PlayerController>())
            {
                if (blockRotation)
                    user.transform.GetComponent<PlayerController>().canRotate = false;
                if (blockMove)
                    user.transform.GetComponent<PlayerController>().canMove = false;
            }

            //Spinta rinculo
            rb.AddForce(-user.transform.forward * rinculo, ForceMode.Impulse);

            //Spara n proiettili
            for (int i = 0; i < bulletNumber; i++)
            {
                //Devia la direzione di sparo in modo casuale in base alla precisione 
                Vector3 direction = new Vector3(shootPoint.transform.forward.x + Random.Range(-10 / precision, 10 / precision), shootPoint.transform.forward.y, shootPoint.transform.forward.z + Random.Range(-10 / precision, 10 / precision));

                //Pool objs technique
                for(int j=0;j<bullets.Count;j++)
                {
                    if(!bullets[j].activeInHierarchy)
                    {
                        bullets[j].transform.position = shootPoint.transform.position;
                        bullets[j].transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                        bullets[j].SetActive(true);
                        break;
                    }
                }

                //Raycast per gunline e gunline
				if (drawLine) 
				{
					Ray ray = new Ray (shootPoint.transform.position, direction);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, range))
						gunLine.SetPosition (1, hit.point);
					else
						gunLine.SetPosition (1, ray.origin + direction * range);
				}
            }

            if (drawLine)
                gunLine.enabled = true;
        }
    }

    public void DisableEffects()
    {
        gunLight.enabled = false;
        faceLight.enabled = false;
        gunLine.enabled = false;
    }
    
    public void setPUFlagsFalse()
    {
        precisionUpFlag = new bool[precisionUpValues.Length];
        for (int i = 0; i < precisionUpFlag.Length; i++)
            precisionUpFlag[i] = false;
        rangeUpFlag = new bool[rangeUpValues.Length];
        for (int i = 0; i < rangeUpFlag.Length; i++)
            rangeUpFlag[i] = false;
    }

    public void poolObjects()
    {
        bullets = new List<GameObject>();
        pooledAmount = (int)range / 5 * bulletNumber;
        GameObject obj;
        for (int i = 0; i < pooledAmount; i++)
        {
            obj = (GameObject)Instantiate(bullet.gameObject, shootPoint.transform.position, transform.rotation);            
            obj.SetActive(false);
            bullets.Add(obj);
            
			Bullet bulletScript = obj.transform.GetComponent<Bullet>();
            bulletScript.weapon = this;
        }
    }
}
