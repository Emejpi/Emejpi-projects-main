using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBFindSquareWithTags : TBBomberWalkNode {

    Square square;

    public override void Play()
    {
        base.Play();

        List<TagWithValue> tags = transform.parent.GetComponent<TBSequencerWithTags>().tags;

        if(Find(tags))
        {
            GetRoot().square = square;
            TaskComplited();
        }
        else
        {
            TaskFailed();
        }
    }

    public bool Find(List<TagWithValue> tags)
    {
        Square mySquare = GetRoot().GetMySquare();

        square = new Square();

        return true;
    }
}
