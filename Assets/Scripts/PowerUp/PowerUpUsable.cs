using UnityEngine;
using System.Collections;

public class PowerUpUsable : MonoBehaviour
{
    public enum type { CloneTrap, Sprint, Bomb };

    PlayerUsePU player;
    public type PUType;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerUsePU>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            player.pickedPU = PUType.ToString();

            Destroy(gameObject);
        }
    }
}
