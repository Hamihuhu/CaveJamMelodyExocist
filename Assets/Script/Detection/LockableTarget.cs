using System;
using UnityEngine;


/// <summary>
/// Class to handle showing lock HUD
/// Also acting as a tag for GameObject that can be locked on
/// </summary>
public class LockableTarget : MonoBehaviour
{
    [SerializeField] private ScriptableSignalHub cameraScriptableSignalHub;
    [SerializeField] private Transform rayCastReceiverTransform;
    [SerializeField] private GameObject lockHUD;

    public Transform RaycastReceiver { get => rayCastReceiverTransform; }

    private void Start()
    {
        lockHUD.SetActive(false);
    }

    private void OnEnable()
    {
        cameraScriptableSignalHub.Get<OnPlayerRequestLockOn>().AddListener(HandlePlayerRequestLockOn);
    }

    private void OnDisable()
    {
        cameraScriptableSignalHub.Get<OnPlayerRequestLockOn>().RemoveListener(HandlePlayerRequestLockOn);
    }

    private void HandlePlayerRequestLockOn(Transform transform)
    {
        // handle showing lock HUD
        if (transform == rayCastReceiverTransform.root)
        {
            // show lock HUD
            lockHUD.SetActive(true);
        }
        else
        {
           // hide lock HUD
            lockHUD.SetActive(false);
        }
    }
}