using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    Rigidbody2D rigid;

    protected Vector2 targetPosition;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
    }

    protected virtual void Move()
    {
        rigid.velocity = (targetPosition - (Vector2)transform.position) * 10;
    }

    // Update is called once per frame
    void Update()
    {
        Move();   
    }
}
