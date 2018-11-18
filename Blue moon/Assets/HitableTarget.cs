using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitableTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Image>().color = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Image>().enabled)
            GetComponent<Image>().color = Color.green;
    }
}
