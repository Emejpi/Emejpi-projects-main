using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMovement : Movement {

    public void GoTo(Square square)
    {
        if (currentSquare.Neighbours.Contains(square))
        {
            Move(square);
            return;
        }

        Move(PathFinderV2.main.FindNextStep(currentSquare, square));
    }

}
