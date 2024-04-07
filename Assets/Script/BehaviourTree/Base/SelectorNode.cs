using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    int currentChildIndex;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        var child = children[currentChildIndex];
        switch (child.Update())
        {
            case State.RUNNING:
                return State.RUNNING;
            
            case State.SUCCESS:
                return State.SUCCESS;

            case State.FAILURE:
                currentChildIndex++;
                if (currentChildIndex == children.Count)
                {
                    return State.FAILURE;
                }
                break;
        }

        return State.RUNNING;
    }
}
