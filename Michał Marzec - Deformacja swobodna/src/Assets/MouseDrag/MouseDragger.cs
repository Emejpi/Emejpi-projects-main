using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragger : MonoBehaviour {


    public float distance;
    public Transform target;
    public Vector3 hitOffset;

    public void SetTarget(Transform target)
    {
        distance = Mathf.Abs(transform.InverseTransformPoint(target.position).z - transform.InverseTransformPoint(Camera.main.transform.position).z);

        //distance = Vector3.Distance(target.position, Camera.main.transform.position);
        this.target = target;
    }

    void Update()
    {
        if (!target)
            return;

        if(Input.GetMouseButtonUp(0))
        {
            target = null;
            return;
        }

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        target.transform.position = objPosition + hitOffset;
    }

}
