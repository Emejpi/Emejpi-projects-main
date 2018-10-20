using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBNode : MonoBehaviour {

    protected TBNode root;

    public virtual void TaskComplited(int index)
    {
        TaskComplited();
    }

    public void TaskComplited()
    {
        print(gameObject.name + " complited");

        transform.parent.GetComponent<TBNode>().TaskComplited(transform.GetSiblingIndex());
    }

    public virtual void TaskFailed(int index)
    {
        TaskFailed();
    }

    public void TaskFailed()
    {
        print(gameObject.name + " failed");

        transform.parent.GetComponent<TBNode>().TaskFailed(transform.GetSiblingIndex());
    }

	public virtual void Play()
    {
        print(gameObject.name + " play");

        if (!transform.parent)
            return;

        TBNode parent = transform.parent.GetComponent<TBNode>();

        if (!parent)
            return;

        if (parent.root)
            root = parent.root;
        else
            root = parent;
    }

    //public TBRoot GetRoot()
    //{
    //    Transform currentTrensform = transform.parent;

    //    while (!currentTrensform.GetComponent<TBRoot>())
    //        currentTrensform = currentTrensform.parent;

    //    return currentTrensform.GetComponent<TBRoot>();
    //}
}
