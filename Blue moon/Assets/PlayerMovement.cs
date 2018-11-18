using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    public float speed;

    Vector2 dir;

    protected override void Move()
    {
        if (Vector2.Distance(transform.position, Input.mousePosition) > 10)
        {
            dir = Input.mousePosition - transform.position;
            //if(dir.magnitude > 30)
            dir = dir.normalized;
            targetPosition += dir * speed * Time.deltaTime;
        }
        else
            targetPosition = Input.mousePosition;
        base.Move();
    }
}
