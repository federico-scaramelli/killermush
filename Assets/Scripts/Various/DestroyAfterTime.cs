using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

    public float timer;

    void OnEnable()
    {
        Invoke("Destroy", timer);
    }

    void OnDisable()
    {
        CancelInvoke("Destroy");
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
