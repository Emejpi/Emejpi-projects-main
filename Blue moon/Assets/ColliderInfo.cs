using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderInfo : MonoBehaviour {

    public float magnitude;

    public NNRepresentative nn;

    public bool hit;

    private void Start()
    {
        hit = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Target"  )
        {
            magnitude = GetComponent<Rigidbody2D>().velocity.magnitude;
            hit = true;
            GetComponent<Image>().color = Color.green;
            nn.GetComponent<Image>().color = Color.green;
            nn.Finish();
        }
    }
}
