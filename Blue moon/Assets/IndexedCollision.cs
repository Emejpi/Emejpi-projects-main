using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexedCollision : MonoBehaviour {

    public GameObject mainParent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == mainParent.name)
        {
            collision.gameObject.GetComponent<Image>().color = Color.red;
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            GetComponent<ColliderInfo>().magnitude = GetComponent<Rigidbody2D>().velocity.magnitude;
            GetComponent<ColliderInfo>().hit = true;
            GetComponent<Image>().color = Color.green;
            mainParent.GetComponent<Image>().color = Color.green;
            mainParent.GetComponent<NNRepresentative>().Finish();
        }
    }
}
