using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBBomberWalkNode : TBNode {

    protected TBBomberWalkRoot GetRoot()
    {
        print(root.gameObject.name);
        return root.GetComponent<TBBomberWalkRoot>();
    }

}
