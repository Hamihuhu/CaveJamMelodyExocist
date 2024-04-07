using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushableTarget : MonoBehaviour
{
    [SerializeField] private Transform rayCastReceiverTransform;
    [SerializeField] private GameObject rushHUD;

    public Transform RaycastReceiver { get => rayCastReceiverTransform; }

    
}
