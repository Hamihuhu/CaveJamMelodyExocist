using deVoid.Utils;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable SignalHub")]
public class ScriptableSignalHub : ScriptableObject
{
    private readonly SignalHub signalHub = new SignalHub();

    public SType Get<SType>() where SType : ISignal, new()
    {
        return signalHub.Get<SType>();
    }

    public void AddListenerToHash(string signalHash, Action handler)
    {
        signalHub.AddListenerToHash(signalHash, handler);
    }

    public void RemoveListenerFromHash(string signalHash, Action handler)
    {
        signalHub.RemoveListenerFromHash(signalHash, handler);
    }
}
