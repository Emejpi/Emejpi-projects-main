using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBParallel : TBComposite {

    public override void Play()
    {
        base.Play();
        foreach (TBNode node in nodes)
            node.Play();
    }
}
