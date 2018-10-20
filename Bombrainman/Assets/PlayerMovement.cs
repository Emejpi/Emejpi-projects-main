using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement {
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.W))
        {
            Move(new Vector2(0, 1));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Move(new Vector2(0, -1));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(new Vector2(1, 0));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(new Vector2(-1, 0));
        }
    }
}
