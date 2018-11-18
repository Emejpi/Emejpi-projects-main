using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointFixDistance : MonoBehaviour {

    public float closingSpeed;

    HingeJoint2D joint;
    float distance;

	// Use this for initialization
	void Start () {
        joint = GetComponent<HingeJoint2D>();
        distance = Vector2.Distance(joint.connectedBody.transform.position, transform.position);

    }
	
	// Update is called once per frame
	void Update () {
        float curDistance = Vector2.Distance(joint.connectedBody.transform.position, transform.position);

        if (curDistance > distance)
        {
            transform.position += (joint.connectedBody.transform.position - transform.position).normalized * closingSpeed * Time.deltaTime * (curDistance - distance);
        }
	}
}
