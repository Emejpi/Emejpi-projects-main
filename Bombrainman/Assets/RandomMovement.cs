using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : Movement {

    private void Update()
    {
        Move(currentSquare.Neighbours[Random.Range(0, currentSquare.Neighbours.Count)]);
    }

}
