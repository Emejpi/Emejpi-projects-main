using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBTag : TBNode {

    public Tag tag;
    public TBConditionType type;

    public override void Play()
    {
        base.Play();

        if (type != TBConditionType.Neutral)
            transform.parent.GetComponent<TBSequencerWithTags>().tags.Add
                    (
                        new TagWithValue
                        {
                            tag = tag,
                            value = (type == TBConditionType.Necessary)
                        }
                     );

        TaskComplited();
    }
}
