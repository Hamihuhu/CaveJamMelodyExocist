using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    int current;

    protected override void OnStart()
    {
        current = 0;

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            case State.RUNNING:
                return State.RUNNING;

            case State.FAILURE:
                return State.FAILURE;

            case State.SUCCESS:
                current++;
                if (current == children.Count)
                {
                    return State.SUCCESS;
                }
                break; 
        }

        return State.RUNNING;
    }
}
