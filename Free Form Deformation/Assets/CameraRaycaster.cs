using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraRaycaster : MonoBehaviour {

    [System.Serializable]
    public class MyEventType : UnityEvent { }

    [Header("On Click")]
    public MyEventType OnEvent;

    public GameObject lastHit;
    public Vector3 hitOffset;

    void FixedUpdate()
    {
        //if mouse button (left hand side) pressed instantiate a raycast
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                Debug.Log(hit.point);

                lastHit = hit.collider.gameObject;

                hitOffset =  hit.collider.transform.position - hit.point;

                OnEvent.Invoke();

            }
        }
    }
}
