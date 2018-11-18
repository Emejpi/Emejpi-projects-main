using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour {

    public int familyID;
    public float fitness;

    public Color color;
    public Color bestColor;

    public Text killsCountText;

    public virtual void Inicialize(Creature creature)
    {
        //set color;
        Inicialize();
    }

    public virtual void Inicialize()
    {
        GetComponent<Image>().color = color;
        fitness = 0;
        GetComponent<SimplaAIHunter>().targets = new List<GameObject>();
        gameObject.SetActive(true);
        GetComponent<Movement>().enabled = true;
        //set color;
    }

    public void BestOne()
    {
        GetComponent<Image>().color = bestColor;
    }

    public virtual void Mutate(Creature creature)
    {

    }

    public void FeedPoints(int points)
    {
        fitness += points;
        killsCountText.text = (int)fitness + "";
    }
}
