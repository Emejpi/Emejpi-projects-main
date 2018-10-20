using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBSequencer : TBComposite {

    public override void Play()
    {
        base.Play();

        if(nodes.Count > 0)
            nodes[0].Play();
    }

    public override void TaskComplited(int index)
    {
        if (index >= nodes.Count - 1)
        {
            TaskComplited();
            return;
        }

        nodes[index + 1].Play();
    }
}
