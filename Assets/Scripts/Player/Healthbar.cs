using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class Healthbar : MonoBehaviour
{

    public RectTransform healthBar;
    public Image healthBarImage;
    private float minXpos;
    private float maxXpos;
    private float cachedY;
    private Health health;

    void Start()
    {
        cachedY = healthBar.transform.position.y;
        maxXpos = healthBar.transform.position.x;
        minXpos = healthBar.transform.position.x - 180;
        health = GetComponent<Health>();
    }
    
    void Update()
    {
        float currentXpos = MapValues(health.currentHealth, 0, health.maxHealth, minXpos, maxXpos);
        healthBar.position = new Vector3(currentXpos, cachedY);

        if (health.currentHealth > health.maxHealth / 2)
        {
            healthBarImage.color = new Color32((byte)MapValues(health.currentHealth, health.maxHealth / 2, health.maxHealth, 255, 0), 255, 0, 255);
        }
        else
        {
            healthBarImage.color = new Color32(255, (byte)MapValues(health.currentHealth, 0, health.maxHealth / 2, 0, 255), 0, 255);
        }
    }

    private float MapValues(float x, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return (x - inputMin) * (outputMax - outputMin) / (inputMax - inputMin) + outputMin;
    }
}
