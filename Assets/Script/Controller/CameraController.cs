using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _targetFollow;
    Vector3 _cameraVelocity = Vector3.zero;
    [field: SerializeField] PlayerInput input;
    [SerializeField] private float cammeraFollowSpeed = 0.2f;
    [SerializeField] private float lookAngle;
    [SerializeField] private float pivotAngle;
    //[SerializeField] private float minLookAngle=-35;
    //[SerializeField] private float maxLookAngle=35;
    [SerializeField] private float _cameraLookSpeed = 15;
   // [SerializeField] private float _cameraPivotSpeed = 15;
    [SerializeField]private float _cameraLookSmoothTime = 1;


    private void Update()
    {
        FollowTarget();
        RotationCamera();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, _targetFollow.position, ref _cameraVelocity, cammeraFollowSpeed);
        transform.position = targetPosition;
    }
    private void RotationCamera()
    {
        lookAngle = Mathf.Lerp(lookAngle, lookAngle + (input.Look.x * _cameraLookSpeed), _cameraLookSmoothTime * Time.deltaTime);
        // pivotAngle = Mathf.Lerp(pivotAngle, pivotAngle - (PlayerInput.Instance.Look.y * _cameraPivotSpeed), _cameraLookSmoothTime * Time.deltaTime);
        //pivotAngle=Mathf.Clamp(pivotAngle, minLookAngle,maxLookAngle);
        //Quaternion targetRotation = Quaternion.Euler(new Vector3(pivoteAngle,lookAngle));
        Quaternion targetRotation = Quaternion.Euler(new Vector3(transform.rotation.x,lookAngle));
        transform.localRotation = targetRotation;
    }
}
