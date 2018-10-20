using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBooming : Booming {

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            Bomb(GetComponent<Movement>().currentSquare);
            transform.SetAsLastSibling();
        }
	}
}
