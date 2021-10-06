using UnityEngine;
using System.Collections;

public class PlayerCloneTrap : MonoBehaviour
{
    WeaponManager manager;
    WeaponManager cloneManager;
    public Transform clonePrefab;
    GameObject[] enemies;

	void Start ()
    {
        manager = GetComponent<WeaponManager>();
	}
	
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(clonePrefab, transform.position, transform.rotation);
            cloneManager = GameObject.FindGameObjectWithTag("PlayerClone").transform.GetComponent<WeaponManager>();
            cloneManager.ChangeWeapon(manager.currentWeapon);
        }
	}
}
