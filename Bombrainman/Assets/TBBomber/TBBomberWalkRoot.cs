using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBBomberWalkRoot : TBRoot {

    public Square square;
    public int maximumDepthReached;

    public void Go()
    {
        if (!square)
            return;

        GetComponent<GoToMovement>().GoTo(square);
    }

    public override void TaskComplited(int index)
    {
        Go();
    }

    public Square GetMySquare()
    {
        return GetComponent<GoToMovement>().currentSquare;
    }
}
