using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : Movement {

    public float timeTillBoom;

    int boomCounter;
    float timeTillBoomSlice;

    public int range;

    public Creature myCreator;

	// Use this for initialization
	void Start () {
        timeTillBoomSlice = timeTillBoom / 5;
        boomCounter = 5;

        Invoke("CountDown", timeTillBoomSlice);
	}

    void CountDown()
    {
        if(GetComponent<Image>().color == Color.red)
        {
            GetComponent<Image>().color = (Color.red + Color.yellow) / 2;
        }
        else
        {
            GetComponent<Image>().color = Color.red;
        }

        boomCounter--;
        if(boomCounter == 0)
        {
            Explode();
        }
        else
            Invoke("CountDown", timeTillBoomSlice);
    }

    public void FeedkillCount(int killCount)
    {
        myCreator.FeedPoints(killCount);
    }

    void Explode()
    {
        int killCount;
        Board.main.Explode(currentSquare, range, out killCount);
        FeedkillCount(killCount);
        Destroy(gameObject);
    }
	
}
