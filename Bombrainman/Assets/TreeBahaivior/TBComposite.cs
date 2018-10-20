using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBComposite : TBNode {

    public List<TBNode> nodes;
    protected List<bool> checkList;

    public override void Play()
    {
        base.Play();

        if(nodes.Count == 0)
        {
            foreach(Transform child in transform)
            {
                TBNode node = child.GetComponent<TBNode>();
                if (node)
                    nodes.Add(node);
            }
        }

        checkList = new List<bool>();

        foreach (TBNode node in nodes)
            checkList.Add(false);
    }

    public override void TaskComplited(int index)
    {
        checkList[index] = true;

        if(!checkList.Contains(false))
            TaskComplited();
    }
}
