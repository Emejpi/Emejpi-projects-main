using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBRoot : TBParallel
{
    public bool autoPlay;

    private void Start()
    {
        if (!autoPlay)
            enabled = false;
    }

    private void Update()
    {
        Play();
    }

    public override void TaskComplited(int index)
    {
        enabled = false;
    }

    public override void TaskFailed(int index)
    {
        enabled = false;
    }
}
