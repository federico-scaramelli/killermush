using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    WeaponManager manager;
    PlayerController controller;

    void Start()
    {
        manager = transform.GetComponent<WeaponManager>();
        manager.ChangeWeapon(manager.startWeapon);
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        //CONTROLLO SPARO
        manager.currentWeapon.timer += Time.deltaTime;

        if (Input.GetButton("Fire1") || Input.GetAxis("PadFire")<-0.6) //Alla pressione del tasto sx del mouse
        {
            if (manager.currentWeapon.firstShot) //Se è il primo sparo
            {
                manager.currentWeapon.timer = 0; //imposto il timer a 0
                manager.currentWeapon.Shoot(); //sparo

                manager.currentWeapon.firstShot = false; //Non è più il primo sparo
            }
            else
            {
                //Se il timer supera i ms da attendere da un colpo e l'altro
                if (manager.currentWeapon.timer >= manager.currentWeapon.msBetweenBullets / 1000 && Time.timeScale != 0)
                {
                    manager.currentWeapon.timer = 0; //Resetto il timer
                    manager.currentWeapon.Shoot(); //sparo
                }
            }
        }

        if (Input.GetButtonUp("Fire1") || Input.GetAxis("PadFire") > -0.6) //Al rilascio del tasto sx del mouse
        {
            if (controller)
            {
                controller.canMove = true;
                controller.canRotate = true;
            }
        }

        /*Se il timer supera i ms da attendere tra un colpo e l'altro moltiplicato
         per il tempo per cui devono rimanere visibili gli effetti*/
        if (manager.currentWeapon.timer >= manager.currentWeapon.msBetweenBullets / 1000 * manager.currentWeapon.effectsTime)
        {
            manager.currentWeapon.DisableEffects(); //Disabilito gli effetti (luci,fiammata)
        }
    }

    void FixedUpdate()
    {
        //Ricavo gli input degli assi tramite pressione delle frecce o di WASD
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0, vertical); //Creo un vettore in base a questi input
        if(movement!=Vector3.zero)
            if (controller.canMove)
                controller.Move(movement); //faccio muovere il player in base al vettore appena calcolato, che gli passo
    }
}