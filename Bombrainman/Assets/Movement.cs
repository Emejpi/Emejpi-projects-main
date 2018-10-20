using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public Square currentSquare;

    public float moveDelta;

    bool ready;

    private void Start()
    {
        Ready();
    }

    void Ready()
    {
        ready = true;
    }

    public void Move(Vector2 vec)
    {
        if(vec.x == 1)
        {
            Move(currentSquare.Right);
        }
        else if(vec.x == -1)
        {
            Move(currentSquare.Left);
        }
        else if (vec.y == 1)
        {
            Move(currentSquare.Up);
        }
        else if (vec.y == -1)
        {
            Move(currentSquare.Down);
        }
    }

    public void Move(Square square)
    {
        if (!ready)
            return;

        if (!square || square.type == Square.Type.wall || square.type == Square.Type.destructable || square.player != null || square.bomb != null)
            return;

        switch(square.type)
        {
            case Square.Type.explosion:
                Board.main.DestroyPlayer(gameObject);
                return;
        }

        transform.SetParent(square.transform, false);

        square.Place(this);

        ready = false;
        Invoke("Ready", moveDelta);
    }
}
