using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode : ScriptableObject
{
    public enum State
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public string guid;
    public State state = State.RUNNING;
    protected bool started = false;
    public Vector2 position;

    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state != State.RUNNING)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
