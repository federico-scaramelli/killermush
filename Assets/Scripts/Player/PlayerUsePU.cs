using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUsePU : MonoBehaviour
{

    public RectTransform pickedPUText;
    public string pickedPU="";

    public Transform clonePrefab;
    public float sprintForce;
    public Transform bomb;
    public Transform shootPoint;
    public float shootForce;

    public KeyCode useKey;
    Rigidbody rigidBody;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
	

	void Update ()
    {
        pickedPUText.GetComponent<Text>().text = pickedPU;
        if (pickedPU!="")
        {
            if (Input.GetKeyDown(useKey))
            {
                switch (pickedPU)
                {
                    case "CloneTrap":
                        rigidBody.AddForce(-transform.forward * 5, ForceMode.Impulse);
                        Instantiate(clonePrefab, transform.position, transform.rotation);
                        pickedPU = "";
                        break;
                    case "Sprint":
                        rigidBody.AddForce(transform.forward * sprintForce, ForceMode.Impulse);
                        pickedPU = "";
                        break;
                    case "Bomb":
                        Transform spawnedBomb=(Transform)Instantiate(bomb, shootPoint.position, Quaternion.identity);
                        spawnedBomb.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Impulse);
                        pickedPU = "";
                        break;
                    default:
                        pickedPUText.GetComponent<Text>().text = "";
                        pickedPU = "";
                        break;
                }
            }
        }
    }
}
