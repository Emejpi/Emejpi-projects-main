using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booming : MonoBehaviour {

    public GameObject bombPref;
    public int range;

	public GameObject Bomb(Square square)
    {
        Movement bomb = Instantiate(bombPref, square.transform).GetComponent<Movement>();
        bomb.currentSquare = square;
        bomb.GetComponent<Bomb>().range = range;
        square.bomb = bomb.gameObject;
        Board.main.ShadowExplode(square, range);

        return bomb.gameObject;
    }
}
