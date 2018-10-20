using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBFirstNotFailed : TBSequencer {

    public override void TaskComplited(int index)
    {
        TaskComplited();
    }

    public override void TaskFailed(int index)
    {
        if (index >= nodes.Count - 1)
        {
            TaskFailed();
            return;
        }

        nodes[index + 1].Play();
    }
}
