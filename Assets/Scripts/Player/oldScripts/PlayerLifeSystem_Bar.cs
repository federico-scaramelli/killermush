using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLifeSystem_Bar : MonoBehaviour
{
    public float maxHealth;
    public RectTransform healthBar;
    public Image healthBarImage;
    public float coolDownTimer;

    private float minXpos;
    private float maxXpos;
    private float cachedY;
    private float currentHealth;
    private bool onCD;


    void Start()
    {
        cachedY = healthBar.transform.position.y;
        currentHealth = maxHealth;
        maxXpos = healthBar.transform.position.x;
        minXpos = healthBar.transform.position.x-healthBar.rect.width;
    }
    
    void Update()
    {
        updateHealth();
        if(!onCD && currentHealth>0)
        {
            StartCoroutine(CoolDownDmg());
            currentHealth -= 1;
        }
        
    }

    void updateHealth()
    {
        float currentXpos = MapValues(currentHealth, 0, maxHealth, minXpos, maxXpos);
        healthBar.position = new Vector3(currentXpos, cachedY);

        if(currentHealth>maxHealth/2)
        {
            healthBarImage.color = new Color32((byte)MapValues(currentHealth,maxHealth/2,maxHealth,255,0), 255, 0, 255);
        }
        else
        {
            healthBarImage.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth/2, 0, 255), 0, 255);
        }
    }

    IEnumerator CoolDownDmg()
    {
        onCD = true;
        yield return new WaitForSeconds(coolDownTimer);
        onCD = false;
    }

    private float MapValues(float x, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return (x - inputMin) * (outputMax - outputMin) / (inputMax - inputMin) + outputMin;
    }
}
