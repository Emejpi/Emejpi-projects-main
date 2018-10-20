using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBBomberWalkNode : TBNode {

    protected TBBomberWalkRoot GetRoot()
    {
        return root.GetComponent<TBBomberWalkRoot>();
    }

}
