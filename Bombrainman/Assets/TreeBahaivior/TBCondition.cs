using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TBConditionType
{
    Neutral,
    Necessary,
    Forbidden
}

public class TBCondition : TBNode {

    public TBConditionType type;

    public override void Play()
    {
        switch(type)
        {
            case TBConditionType.Neutral:
                TaskComplited();
                return;

            case TBConditionType.Necessary:
                if(Condition())
                {
                    TaskComplited();
                }
                else
                {
                    TaskFailed();
                }
                return;

            case TBConditionType.Forbidden:
                if (Condition())
                {
                    TaskFailed();
                }
                else
                {
                    TaskComplited();
                }
                return;
        }
    }

    protected virtual bool Condition()
    {
        return true;
    }
}
