using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBRandomSelector : TBComposite {

    public override void Play()
    {
        base.Play();
        if(nodes.Count > 0)
        {
            nodes[Random.Range(0, nodes.Count)].Play();
        }
    }

    public override void TaskComplited(int index)
    {
        TaskComplited();
    }


}
