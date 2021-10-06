using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    public float rotSpeed;

    void Start()
    {
        if(rotSpeed==0)
        {
            rotSpeed = 70;
        }
    }

	void Update ()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }
}
