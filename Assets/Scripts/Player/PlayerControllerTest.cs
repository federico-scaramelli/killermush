﻿using UnityEngine;
using System.Collections;

public class PlayerControllerTest : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public bool speedDown = false;
    public bool canRotate = true;
    public bool canMove = true;
    public float rotSpeed;
    WeaponManagerTest manager;
    Vector3 lookPos;
    GameObject[] enemies;
    Rigidbody rigidBody;
    string[] joypads;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        manager = GetComponent<WeaponManagerTest>();
    }

    void Update()
    {
        /*joypads = Input.GetJoystickNames();

        if (joypads.Length > 0)
        {
            lookPos = new Vector3(Input.GetAxis("Horizontal2"), 0, Input.GetAxis("Vertical2"));
            Debug.Log("X: " + lookPos.x + "; Y:" + lookPos.y + "; Z:" + lookPos.z + ";");
            if (lookPos.x != 0 || lookPos.z != 0)
            {
                if (canRotate)
                {
                    /*Quaternion rotation = Quaternion.LookRotation(lookPos);
                    rotation.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);*/
                    /*
                    transform.LookAt(transform.position + lookPos, Vector3.up);

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
            }
        }*/

    }

    //Funzione di movimento che "spinge" il player in base al vettore che gli si passa e alla velocità impostata
    public void Move(Vector3 _movement)
    {
        rigidBody.AddForce(_movement * speed / Time.fixedDeltaTime, ForceMode.Force);
        //rigidBody.MovePosition(transform.position + _movement * speed * Time.deltaTime);
    }

    public void Rotate(Vector3 lookPos)
    {
        if (canRotate)
            transform.LookAt(lookPos, Vector3.up);
    }

    //Alla distruzione (morte), imposta il target dei nemici su se stessi in quanto non c'è più un player in campo
    void OnDestroy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SendMessage("ChangeTarget", enemy.transform, SendMessageOptions.DontRequireReceiver);
        }
    }
}