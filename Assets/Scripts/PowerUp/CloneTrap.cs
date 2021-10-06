using UnityEngine;
using System.Collections;

public class CloneTrap : MonoBehaviour
{
    GameObject[] enemies;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SendMessage("ChangeTarget", transform, SendMessageOptions.RequireReceiver);
        }
    }

    void OnDestroy ()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SendMessage("ChangeTarget", GameObject.FindGameObjectWithTag("Player").transform, SendMessageOptions.RequireReceiver); 
        }     
    }
}
