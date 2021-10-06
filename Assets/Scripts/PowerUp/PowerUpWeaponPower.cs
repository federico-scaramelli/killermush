using UnityEngine;
using System.Collections;

public class PowerUpWeaponPower : MonoBehaviour
{
    public enum types { Precision, Range };
    public types type;
    private string stringType;
    private float precisionValue;
    private float rangeValue;
    //public float timer;
    //private bool timerStarted = false;
    private float oldValue;
    GameObject player;
    WeaponManager manager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        manager = player.GetComponent<WeaponManager>();
        stringType = type.ToString();
    }

    void Update()
    {
        //if (timerStarted)
        //{
        //    timer -= Time.deltaTime;
        //    if (timer <= 0)
        //    {
        //        switch (stringType)
        //        {
        //            case "Precision":
        //                manager.currentWeapon.precision = oldValue;
        //                break;
        //        }
        //        GameObject.Destroy(gameObject);
        //    }
        //}

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            /*if (timer != 0)
                //timerStarted = true;
            else
                Destroy(gameObject);*/

            switch (stringType)
            {
                case "Precision":
                    precisionValue = manager.currentWeapon.precision;
                    for (int i = 0; i < manager.currentWeapon.precisionUpValues.Length; i++)
                    {
                        if (manager.currentWeapon.precisionUpFlag[i] == false)
                        {
                            precisionValue = manager.currentWeapon.precisionUpValues[i];
                            manager.currentWeapon.precisionUpFlag[i] = true;
                            break;
                        }
                    }
                    manager.currentWeapon.precision=precisionValue;
                    break;
                case "Range":
                    rangeValue = manager.currentWeapon.range;
                    for (int i = 0; i < manager.currentWeapon.rangeUpValues.Length; i++)
                    {
                        if (manager.currentWeapon.rangeUpFlag[i] == false)
                        {
                            rangeValue = manager.currentWeapon.rangeUpValues[i];
                            manager.currentWeapon.rangeUpFlag[i] = true;
                            //foreach (GameObject obj in manager.currentWeapon.bullets)
                            //  GameObject.Destroy(obj);
                            //manager.currentWeapon.poolObjects();
                            break;
                        }
                    }
                    manager.currentWeapon.range = rangeValue;
                    break;
            }
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            //transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            GetComponent<DestroyAfterTime>().enabled = false;
        }
    }
}
