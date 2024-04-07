using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : TaskNode
{
    public string message;

    protected override void OnStart()
    {
        Debug.Log($"DebugLogNode OnStart: {message}");
    }

    protected override void OnStop()
    {
        Debug.Log($"DebugLogNode OnStop: {message}");
    }

    protected override State OnUpdate()
    {
        Debug.Log($"DebugLogNode OnUpdate: {message}");
        return State.SUCCESS;
    }
}
