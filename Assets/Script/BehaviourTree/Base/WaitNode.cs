using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : TaskNode
{
    [SerializeField] protected float duration;
    protected float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return State.SUCCESS;
        }

        return State.RUNNING;
    }
}
