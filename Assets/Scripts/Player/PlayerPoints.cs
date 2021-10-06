using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerPoints : MonoBehaviour
{
    float points;
    public Text pointsText;

    void Start()
    {
        pointsText.text = points.ToString();
    }

    public void AddPoints(float amount)
    {
        points += amount;
        pointsText.text = points.ToString();
    }

    public void RemovePoints(float amount)
    {
        points -= amount;
        pointsText.text = points.ToString();
    }
}
