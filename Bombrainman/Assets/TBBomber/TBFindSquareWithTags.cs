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

        TagWithValue currentTag;

        if (PathFinderV2.main.FindSquareWithTags(mySquare, 10, tags, out square))
        {
            return true;
        }

        square = new Square();

        return false;
    }

    bool Contains(List<TagWithValue> tags, Tag tag, out TagWithValue tagToWorkOn)
    {
        foreach (TagWithValue tagWV in tags)
            if (tagWV.tag == tag)
            {
                tagToWorkOn = tagWV;
                return true;
            }

        tagToWorkOn = new TagWithValue();

        return false;
    }


}
