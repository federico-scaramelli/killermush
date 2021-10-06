using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float coolDownTimer; //Tempo tra una riduzione della vita e un'altra  
    public float currentHealth;
    public float resistance;
    public bool deactiveOnDeath=false;
    public bool hideOnDeath = false;
    private bool onCD; //Determina se si sta attendendo il passare del tempo di attesa tra una riduzione della vita e un'altra  
    public bool damageable=true; 
    private DropPowerUp drop;
    private bool dropped=false;
    public float healthBarShowTime; //Indica quanti secondi rimarrà visibile la healthbar posizionata sulla testa dell'oggetto interessato dopo una riduzione della vita
    public Transform healthBarCanvas;
    public Image healthBarImage;

    private GameObject[] enemies;

    void Awake()
    {
        drop = GetComponent<DropPowerUp>();
        if(GetComponentInChildren<Canvas>())
            healthBarCanvas = GetComponentInChildren<Canvas>().transform;
        resistance = 1;
    }

    void Start()
    {
        currentHealth = maxHealth;
        if(healthBarShowTime>0)
            healthBarCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        currentHealth=Mathf.Clamp(currentHealth, 0, maxHealth);
        if(healthBarImage)
            healthBarImage.fillAmount = currentHealth / maxHealth;
    }
    
    IEnumerator CoolDownDmg()
    {
        onCD = true;
        yield return new WaitForSeconds(coolDownTimer);
        onCD = false;
    }
    
    IEnumerator ShowHealthBar()
    {
        healthBarCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(healthBarShowTime);
        healthBarCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Applica una riduzione della vita dell'oggetto colpito, se esso ne ha una
    /// </summary>
    /// <param name="damage">Vita da sottrarre</param>
    public void Damage(float damage)
    {
        if (!onCD && currentHealth > 0 && damageable)
        {
            StartCoroutine(CoolDownDmg());
            currentHealth -= damage/resistance;
        }
        if (currentHealth <= 0)
        {
            if (drop!=null && dropped==false)
            {
                drop.Drop();
                dropped = true;
            }

            if (!deactiveOnDeath && !hideOnDeath)
                Destroy(gameObject);
            else if (deactiveOnDeath)
            {
                gameObject.SetActive(false);
                
                if (transform.tag == ""/*"Player"*/)
                {
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                    {
                        enemy.SendMessage("Stop", SendMessageOptions.RequireReceiver);
                    }
                    gameObject.SetActive(false);
                    gameObject.SetActive(true);
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                    {
                        enemy.SendMessage("Restart", SendMessageOptions.RequireReceiver);
                    }
                    return;
                }
            }
            else if (hideOnDeath)
            {
                if (transform.GetComponent<MeshRenderer>())
                    transform.GetComponent<MeshRenderer>().enabled = false;
                if (transform.GetComponent<Collider>())
                    transform.GetComponent<Collider>().enabled = false;
                
                for (int i = 0;i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).name=="Graphic" || transform.GetChild(i).name == "Body")
                    {
                        for(int k = 0; k < transform.GetChild(i).childCount; k++)
                        {
                            if (transform.GetChild(i).GetChild(k).name=="Graphic")
                            {
                                for (int j = 0; j < transform.GetChild(i).GetChild(k).childCount; j++)
                                {
                                    if (transform.GetChild(i).GetChild(k).GetChild(j).GetComponent<MeshRenderer>())
                                        transform.GetChild(i).GetChild(k).GetChild(j).GetComponent<MeshRenderer>().enabled = false;
                                    if (transform.GetChild(i).GetChild(k).GetChild(j).GetComponent<Collider>())
                                        transform.GetChild(i).GetChild(k).GetChild(j).GetComponent<Collider>().enabled = false;
                                }
                            }

                            if (transform.GetChild(i).GetChild(k).GetComponent<MeshRenderer>())
                                transform.GetChild(i).GetChild(k).GetComponent<MeshRenderer>().enabled = false;
                            if (transform.GetChild(i).GetChild(k).GetComponent<Collider>())
                                transform.GetChild(i).GetChild(k).GetComponent<Collider>().enabled = false;
                        }
                    }

                    if(transform.GetChild(i).GetComponent<MeshRenderer>())
                        transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                    if(transform.GetChild(i).GetComponent<Collider>())
                        transform.GetChild(i).GetComponent<Collider>().enabled = false;
                    if (transform.GetChild(i).transform)
                        transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (healthBarShowTime > 0 && healthBarImage && healthBarCanvas)
                StartCoroutine(ShowHealthBar());
        }
    }
}
