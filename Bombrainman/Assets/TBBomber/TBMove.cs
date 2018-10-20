using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBMove : TBBomberWalkNode {

    public override void Play()
    {
        base.Play();
        GetRoot().Go();

        TaskComplited();
    }
}
