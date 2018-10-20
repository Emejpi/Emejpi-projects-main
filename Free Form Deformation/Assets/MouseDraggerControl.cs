using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDraggerControl : MouseDragger {

	public void SetTarget()
    {
        target = Camera.main.GetComponent<CameraRaycaster>().lastHit.transform;
        hitOffset = Camera.main.GetComponent<CameraRaycaster>().hitOffset;
        if (target.gameObject.name != "CP")
            target = null;
        else
            SetTarget(target);
    }

}
