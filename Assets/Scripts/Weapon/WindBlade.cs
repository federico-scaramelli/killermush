using UnityEngine;
using System.Collections;

public class WindBlade : MonoBehaviour {
    public float speed; //Velocità del proiettile
    public GameObject bloodEffect;
    [HideInInspector]
    public RaycastHit hit;
    [HideInInspector]
    public Weapon weapon;

    private float timeLife;

    private float moveDistance;
    private GameObject player;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timeLife = player.GetComponent<WeaponManager>().currentWeapon.range / speed; //Calcolo il tempo di vita dell'oggetto dividendo range dell'arma con velocità del proiettile (t=s/v)
        Invoke("Destroy", timeLife);
    }
	
	void Update ()
    {
        moveDistance = speed * Time.deltaTime; //Calcolo la distanza effettuata in un certo momento
        CheckCollision();
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void CheckCollision()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        //Verifico se il proiettile tocca qualcosa con un raycast che parte dal punto di origine del proiettile e si allunga fino al punto raggiunto
        if (Physics.Raycast(ray, out hit, moveDistance))
        {
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal); //Ricavo l'angolo con cui il proiettile ha colpito 

            Health hitHealth = hit.transform.GetComponentInParent<Health>();

            //Tolgo vita a chi viene colpito
            if (hitHealth)
            {
                if (hitHealth.damageable)
                   hitHealth.Damage(weapon.damage);
            }

            //Spinta
            if (hit.transform.GetComponentInParent<Rigidbody>() || hit.transform.GetComponent<Rigidbody>())
            {
                if(hit.transform.tag=="Enemy" || hit.transform.parent.tag=="Enemy")
                    Instantiate(bloodEffect, hit.transform.position, hitRotation);
                Vector3 force = transform.forward * weapon.spinta;
                if(hit.rigidbody)
                    hit.rigidbody.AddForceAtPosition(force, hit.point, ForceMode.Impulse);
            }

            gameObject.SetActive(false);
        }
    }

    void Destroy()
    {
        gameObject.SetActive(false); 
    }

    void OnDisable()
    {
        CancelInvoke("Destroy");
    }
}
