using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : Movement {

    public float speed;

    Vector2 dir;



    public void Move(Vector2 dir)
    {
        this.dir = dir;
    }

    protected override void Move()
    {
        targetPosition += dir * speed * Time.deltaTime;
        base.Move();
    }
}
