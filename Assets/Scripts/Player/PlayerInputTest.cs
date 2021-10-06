using UnityEngine;
using System.Collections;

public class PlayerInputTest : MonoBehaviour
{
	WeaponManagerTest manager;
	PlayerControllerTest controller;
    //string[] joypads;
    int floorMask;
    Vector3 lookPos;
    Ray camRay;
    Vector3 lookDir;
    RaycastHit hit;

    void Start()
	{
		manager = transform.GetComponent<WeaponManagerTest>();
		controller = GetComponent<PlayerControllerTest>();
        floorMask = LayerMask.GetMask("Floor");
    }

	void Update()
	{
        //joypads = Input.GetJoystickNames();

        //ROTATE INPUT
        //Effettuo un raycast verso il pavimento di gioco dalla posizione del mouse
        camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camRay, out hit, 100, floorMask))
        {
            lookPos = hit.point;
        }
        //Calcolo la posizione verso la quale deve girarsi il player e azzero la rotazione sull'asse y (rotazione verticale)
        lookDir = lookPos - transform.position;
        lookDir.y = 0;
        lookPos = transform.position + lookDir;
        controller.Rotate(lookPos);


        //ATTACK INPUT
        /*if (joypads.Length > 0)
        {
            if (Input.GetAxis("PadFire") < -0.6 && manager.currentWeapon.firstShot)
            {
                manager.currentWeapon.StartToShot();
            }
            if (Input.GetAxis("PadFire") > -0.6) 
            {
                manager.currentWeapon.StopShot();
                manager.currentWeapon.firstShot = true;
            }
        }*/

        if (Input.GetButtonDown ("Fire1") && manager.currentWeapon.firstShot && manager.currentWeapon.canShot) 
		{
            manager.currentWeapon.StartToShot();
		}
		if (Input.GetButtonUp("Fire1")) //Al rilascio del tasto sx del mouse
		{
			manager.currentWeapon.StopShot ();
			manager.currentWeapon.firstShot = true;
		}
	}

	void FixedUpdate()
	{
        //MOVEMENT INPUT    
		//Ricavo gli input degli assi tramite pressione delle frecce o di WASD
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		Vector3 movement = new Vector3(horizontal, 0, vertical); //Creo un vettore in base a questi input
		if(movement!=Vector3.zero)
		    if (controller.canMove)
			    controller.Move(movement); //faccio muovere il player in base al vettore appena calcolato, che gli passo
	}
}