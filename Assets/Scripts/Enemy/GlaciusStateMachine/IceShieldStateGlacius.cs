using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IceShieldStateGlacius : IGlaciusState {

    private readonly StatePatternGlacius glacius;

    private float health;
    private float maxHealth;
    private Image healthImage;
    private bool healthReset=false;
    private int splintNumber; //Scheggie/Frammenti

    public IceShieldStateGlacius(StatePatternGlacius statePatternGlacius)
    {
        glacius = statePatternGlacius;
    }

    public void UpdateState()
    {
        if (glacius.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().enabled == false) //Se il player distrugge lo scudo
        {
            //Glacius ancora ghiacciato ma scudo distrutto
            glacius.myHealth.resistance = glacius.shieldResistance;
            glacius.transform.GetChild(0).gameObject.SetActive(false);
            glacius.myHealth.healthBarImage = healthImage;
            glacius.transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(false);
            glacius.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
            glacius.myHealth.hideOnDeath = false;
            SetVisible();

            glacius.myHealth.maxHealth = maxHealth;
            glacius.myHealth.currentHealth = health;
            healthReset = true;
        }


        glacius.timer2 -= Time.deltaTime;
        if (glacius.timer2 <= 0)
        {
            if(glacius.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                //Scheggie di ghiaccio
                splintNumber = Random.Range(2, 8);
                for (int i = 0; i < splintNumber; i++)
                {
                    //Instantiate
                }
                glacius.myHealth.resistance = 1;
                glacius.transform.GetChild(0).gameObject.SetActive(false);
                glacius.myHealth.healthBarImage = healthImage;
                glacius.transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(false);
                glacius.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
                glacius.myHealth.hideOnDeath = false;
                if(!healthReset)
                {
                    glacius.myHealth.maxHealth = maxHealth;
                    glacius.myHealth.currentHealth = health;
                }
            }
            ToApproachState();
        }

        glacius.timer3 += Time.deltaTime;
        if(glacius.timer3 >= 1)
        {
            health += glacius.shieldHealthRegenPerSecond;
            glacius.timer3 = 0;
        }
    }

    public void EnterState()
    {   
        health = glacius.myHealth.currentHealth;
        maxHealth = glacius.myHealth.maxHealth;
        healthImage = glacius.myHealth.healthBarImage;

        glacius.transform.GetChild(0).gameObject.SetActive(true); //Scudo attivo
        glacius.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        glacius.transform.GetChild(0).GetComponent<Collider>().enabled = true;

        glacius.myHealth.healthBarImage = glacius.shieldBarImage; //Cambio barra dell'energia da healthBar a shieldBar
        glacius.myHealth.maxHealth = glacius.shieldHealth; //Imposto la vita massima uguale alla vita dello shield
        glacius.myHealth.currentHealth = glacius.myHealth.maxHealth;
        glacius.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false); //Disattivo l'immagine healthBar
        glacius.transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true); //Attivo l'immagine shieldBar

        glacius.timer2 = glacius.shieldTimer;
        glacius.timer3 = 0;
    }

    public void FixedUpdateState()
    {

    }

    public void ToApproachState()
    {
        glacius.approachState.EnterState();
        glacius.currentState = glacius.approachState;
    }

    public void ToDistanceAttackState()
    {
        glacius.distanceAttackState.EnterState();
        glacius.currentState = glacius.distanceAttackState;
    }

    public void ToMeleeAttackState()
    {
        glacius.meleeAttackState.EnterState();
        glacius.currentState = glacius.meleeAttackState;
    }

    public void ToIceShieldState()
    {
        glacius.iceShieldState.EnterState();
        glacius.currentState = glacius.iceShieldState;
    }

    private void SetVisible()
    {
        for(int i = 0; i < glacius.transform.GetChild(1).GetChild(0).childCount; i++) // Glacius(transform) \ Body(child 1) \ Graphic(child 0)
        {
            if(glacius.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<MeshRenderer>())
                glacius.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            if(glacius.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Collider>())
                glacius.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Collider>().enabled = true;
        }
        
        for (int i = 0; i < glacius.transform.childCount; i++)
        {
            if (glacius.transform.GetChild(i).transform)
                glacius.transform.GetChild(i).transform.gameObject.SetActive(true);
        }
        
    }
}