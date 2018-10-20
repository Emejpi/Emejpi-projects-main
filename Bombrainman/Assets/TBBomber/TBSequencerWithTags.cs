using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBSequencerWithTags : TBSequencer {

    public List<TagWithValue> tags;

    public override void Play()
    {
        tags = new List<TagWithValue>();
        base.Play();
    }

}
