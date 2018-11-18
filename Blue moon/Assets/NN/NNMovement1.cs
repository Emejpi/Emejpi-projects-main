using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NNMAttack : NNHandle {

    AIMovement movement;

    public Transform blueMoon;

    public bool training;

    public override void OnStart()
    {
        base.OnStart();

        movement = GetComponent<AIMovement>();

        if (net == null || !target)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
        float[] inputs = new float[3];

        //set inputs
        //Vector2 vecToTarget = (target.position -transform.position).normalized;
        //inputs[0] = vecToTarget.x;
        //inputs[1] = vecToTarget.y;

        Vector2 vecToTargetFromBM = (target.position - blueMoon.transform.position);
        inputs[0] = vecToTargetFromBM.x;
        inputs[1] = vecToTargetFromBM.y;

        inputs[2] = Vector2.Distance(transform.position, target.position) > 200? -1 : 1;

        //inputs[3] = transform.position.x > Screen.width - 100 ? -1 : (transform.position.x < 100 ? 1 : 0);
        //inputs[4] = transform.position.y > Screen.height - 100 ? -1 : (transform.position.y < 100 ? 1 : 0);


        float[] outputs = net.FeedForward(inputs);

        Vector2 moveDir = ((Vector2)(target.position - blueMoon.position).normalized + new Vector2(outputs[0], outputs[1]).normalized * 2).normalized;
        movement.Move(moveDir);


        if (!training)
            return;

        //if (transform.position.x >= target.transform.position.x - 100 && transform.position.x <= target.transform.position.x + 100)
        if(Vector2.Distance(transform.position, target.transform.position) < 100)
            //|| transform.position.x > Screen.width
            //|| transform.position.x < 0
            //|| transform.position.y > Screen.height
            //|| transform.position.y < 0)
        {
            SetFitness();
            net.SetFitness(GetTime());

            GetComponent<Image>().color = Color.red;
        }

        //if (Vector2.Distance(transform.position, target.position) < 5)
        //{
        //    SetFitness();

        //}
    }

    public override void SetFitness()
    {
        enabled = false;
        movement.enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //calc fitness
        net.SetFitness((1 / GetTime() + (1 / Vector2.Distance(blueMoon.position, target.position)) + blueMoon.GetComponent<ColliderInfo>().magnitude));

        base.SetFitness();
    }
}
