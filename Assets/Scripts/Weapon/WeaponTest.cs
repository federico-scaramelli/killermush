using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Classe che rappresenta un'arma di gioco in ogni sua componente (Grafica+Logica+Dati)
/// </summary>
[System.Serializable]
public class WeaponTest : MonoBehaviour
{
	[HideInInspector] public GameObject shootPoint;
	[HideInInspector] public Light faceLight;
	[HideInInspector] public ParticleSystem muzzleFlash;
	[HideInInspector] public Light gunLight;
	[HideInInspector] public LineRenderer gunLine;

	public string weaponName;
    public float msToStartShooting;
    public float msToEndShooting;
    public float effectsTime;
	public BulletTest bullet;
	public StaticBullet staticBullet;
	public bool canShot;
	public bool blockRotation;
	public bool blockMove;
	public float damage;
	public float range;
	public float precision;
	public float msBetweenBullets;
	public int bulletNumber;
	public float rinculo;
	public float spinta;
	public bool drawLine;
    public bool isShooting = false;
    public int pooledAmount;

    //Vettori per power up potenziamento arma. Il primo rappresenta i valori che i powerup assegnano alle caratteristiche della tua arma;
    public int[] precisionUpValues;
	// il secondo i flag che ne verificano l'attivazione.
	[HideInInspector] public bool[] precisionUpFlag;
	public int[] rangeUpValues;
	[HideInInspector] public bool[] rangeUpFlag;

	[HideInInspector] public List<GameObject> bullets;
	[HideInInspector] public bool firstShot=true;
	[HideInInspector] public float timer;
    [HideInInspector] public float startShotTimer;
    [HideInInspector] public float endShotTimer;
    Rigidbody rb;
	Transform user;
	PlayerController controller;

	public void Start()
	{
		user = transform.parent;
		rb = user.transform.GetComponent<Rigidbody> ();
		controller = GetComponent<PlayerController>();

		poolObjects();

		setPUFlagsFalse();
	}

    public void StartToShot()
    {
        if (!isShooting)
        { 
            isShooting = true;
            startShotTimer = msToStartShooting/1000;
        }
    }

	public void StopShot()
	{
		if (isShooting) 
		{
			isShooting = false;
            canShot = false;
            endShotTimer = msToEndShooting / 1000;
			if (controller) {
				controller.canMove = true;
				controller.canRotate = true;
			}
		}
	}

	public void Update()
	{
		if (isShooting) 
		{
            if(startShotTimer>0)
            {
                startShotTimer -= Time.deltaTime;
            }else{
                timer += Time.deltaTime;

                if (firstShot) //Se è il primo sparo
                {
                    timer = 0; //imposto il timer a 0
                    Shoot(); //sparo

                    firstShot = false; //Non è più il primo sparo
                }
                else
                {
                    //Se il timer supera i ms da attendere da un colpo e l'altro
                    if (timer >= msBetweenBullets / 1000 && Time.timeScale != 0)
                    {
                        timer = 0; //Resetto il timer
                        Shoot(); //sparo
                    }
                }

                /*Se il timer supera i ms da attendere tra un colpo e l'altro moltiplicato
                per il tempo per cui devono rimanere visibili gli effetti*/
                if (timer >= msBetweenBullets / 1000 * effectsTime)
                {
                    DisableEffects(); //Disabilito gli effetti (luci,fiammata)
                }
            }
        }
        else
        {
            DisableEffects();
            if (endShotTimer > 0)
            {
                endShotTimer -= Time.deltaTime;
            }
            else
            {
                canShot = true;
            }
        }
	}

	public void Shoot()
	{
		if (canShot) {
			//Attiva effetti
			gunLight.enabled = true;
			muzzleFlash.Stop ();
			muzzleFlash.Play ();
			faceLight.enabled = true;
			gunLine.SetPosition (0, shootPoint.transform.position);

			//Blocco rotazione/movimento
			if (user.transform.GetComponent<PlayerController> ()) {
				if (blockRotation)
					user.transform.GetComponent<PlayerController> ().canRotate = false;
				if (blockMove)
					user.transform.GetComponent<PlayerController> ().canMove = false;
			}

			//Spinta rinculo
			rb.AddForce (-user.transform.forward * rinculo, ForceMode.Impulse);

			//Spara n proiettili
			for (int i = 0; i < bulletNumber; i++) {
				//Devia la direzione di sparo in modo casuale in base alla precisione 
				Vector3 direction = new Vector3 (shootPoint.transform.forward.x + Random.Range (-10 / precision, 10 / precision), shootPoint.transform.forward.y, shootPoint.transform.forward.z + Random.Range (-10 / precision, 10 / precision));

				//Pool objs technique
				for (int j = 0; j < bullets.Count; j++) {
					if (!bullets [j].activeInHierarchy) {
						bullets [j].transform.position = shootPoint.transform.position;
						bullets [j].transform.rotation = Quaternion.LookRotation (direction, Vector3.up);
						bullets [j].SetActive (true);
						break;
					}
				}

				//Raycast per gunline e gunline
				Ray ray = new Ray (shootPoint.transform.position, direction);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, range))
					gunLine.SetPosition (1, hit.point);
				else
					gunLine.SetPosition (1, ray.origin + direction * range);
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
        GameObject obj;
        //La formula moltiplica il numero dei proiettili da sparare ad ogni shot per l'intero risultante dall'arrotondamento per eccesso
        //del rapporto tra il tempo di vita del proiettile (s/t) e il tempo di attesa tra uno shot e l'altro (ms/1000)
        pooledAmount = bulletNumber * (int)System.Math.Ceiling((range / bullet.speed / (msBetweenBullets / 1000)));
		
		for (int i = 0; i < pooledAmount; i++)
		{
			//if(bullet != null)
				obj = (GameObject)Instantiate(bullet.gameObject, shootPoint.transform.position, transform.rotation);
			//else
				//obj = (GameObject)Instantiate(staticBullet.gameObject, shootPoint.transform.position, transform.rotation);

			obj.SetActive(false);
			bullets.Add(obj);

			BulletTest bulletScript = obj.transform.GetComponent<BulletTest>();
			bulletScript.weapon = this;
			/*StaticBullet staticBulletScript = obj.transform.GetComponent<StaticBullet>();
            staticBulletScript.weapon = this;*/
		}
	}
}
