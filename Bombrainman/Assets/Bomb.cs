using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : Movement {

    public float timeTillBoom;

    int boomCounter;
    float timeTillBoomSlice;

    public int range;

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

    void Explode()
    {
        Board.main.Explode(currentSquare, range);
        Destroy(gameObject);
    }
	
}
