using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

    double deltaTime = 0.0;
    double fps = 0.0;

    public Toggle toggle;

    void Update()
    {
        deltaTime += Time.deltaTime;
        deltaTime /= 2.0;
        fps = 1.0 / deltaTime;

        if(fps < 20)
        {
            GetComponent<Slider>().value -= 0.1f;
        }
    }

    public void Toggle()
    {
        enabled = toggle.isOn;
    }
}
