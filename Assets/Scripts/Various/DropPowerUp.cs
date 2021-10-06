using UnityEngine;
using System.Collections;

public class DropPowerUp : MonoBehaviour
{
    public float dropFrequency;

    PUType PUType;
    public PUType[] types;
    [HideInInspector]
    public PUType typeMin,typeMax; //typeMin=minimo tra gli scelti
    PUType chosenType;

    PowerUp PU;
    PowerUp PUMin, PUMax, chosenPU;

    public void Drop ()
    {
        if (Random.Range(1, 101) <= dropFrequency) //Il power up verr� droppato?
        {
            ChooseType(); //Pescaggio tipo powerup
            ChoosePU(); //Pescaggio powerup
            if (PU!=null)
                Instantiate(chosenPU.PUTransform, transform.position, Quaternion.identity); //Spawna il PU
        }
    }

    void ChooseType()
    {
        typeMin.name = "";
        typeMax.name = "";
        for (int i = 0; i < types.Length; i++)
        {
            PUType = types[i];
            if (Random.Range(1, 101) < PUType.dropFrequency) //Quali tipi verranno pescati?
            {
                PUType.chosen = true;
                if (typeMin.name == "") //Inizializzo il primo PUMin e lo imposto come tipo scelto
                {
                    typeMin = types[i];
                    chosenType = typeMin;
                }
                else if (PUType.dropFrequency == typeMin.dropFrequency) //Se due tipi hanno stessa frequenza..
                {
                    if (Random.Range(0, 2) == 1) //..casuale
                        PUType = types[i];
                }
                if (PUType.dropFrequency < typeMin.dropFrequency) //Controllo se c'è un nuovo typeMin 
                {
                    typeMin = types[i];
                    chosenType = typeMin;
                }
            }
            if (typeMax.name == "")
                typeMax = types[i];
            else if (PUType.dropFrequency == typeMax.dropFrequency)
            {
                if (Random.Range(0, 2) == 1)
                    typeMax = types[i];
            }
            if (PUType.dropFrequency > typeMax.dropFrequency)
            {
                typeMax = types[i];
            }
        }
        //Seleziona il tipo con frequenza più alta se non ne viene pescato nessuno con frequenza inferiore
        if (typeMin.name == "")
            chosenType = typeMax;
    }

    void ChoosePU()
    {
        for (int i = 0; i < chosenType.powerUps.Length; i++)
        {
            PU = chosenType.powerUps[i];
            if (Random.Range(1, 101) < PU.dropFrequency) //Quali powerup verranno pescati?
            {
                PU.chosen = true;
                if (PUMin==null) //Inizializzo il primo PUMin e lo imposto come PU
                {
                    PUMin = chosenType.powerUps[i];
                    chosenPU = PUMin;
                }
                else if (PU.dropFrequency == PUMin.dropFrequency) //Se due PU hanno stessa frequenza..
                {
                    if (Random.Range(0, 2) == 1) //..casuale
                        PUMin = chosenType.powerUps[i];
                }
                if (PU.dropFrequency < PUMin.dropFrequency) //Controllo se c'è un nuovo PUMin 
                {
                    PUMin = chosenType.powerUps[i];
                    chosenPU = PUMin;
                }
            }
            if (PUMax==null)
                PUMax = chosenType.powerUps[i];
            else if (PU.dropFrequency == PUMax.dropFrequency)
            {
                if (Random.Range(0, 2) == 1)
                    PUMax = chosenType.powerUps[i];
            }
            if (PU.dropFrequency > PUMax.dropFrequency)
            {
                PUMax = chosenType.powerUps[i];
            }
        }
        //Seleziona il PU con frequenza più alta se non ne viene pescato nessuno con frequenza inferiore
        if (PUMin==null)
            chosenPU = PUMax;
    }

}


[System.Serializable]
public class PUType
{
    public string name;
    public int dropFrequency;
    [HideInInspector]
    public bool chosen=false;
    public PowerUp[] powerUps;
}

[System.Serializable]
public class PowerUp
{
    public string name;
    public Transform PUTransform;
    public int dropFrequency;
    [HideInInspector]
    public bool chosen = false;
}
