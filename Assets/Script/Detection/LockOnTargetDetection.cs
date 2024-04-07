using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTargetDetection : MonoBehaviour
{
    [SerializeField] private ScriptableSignalHub cameraScriptableSignalHub;
    [SerializeField] private Transform followTransform;
    [SerializeField] private Transform raycastTransform;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float maximumViewableAngle;
    [SerializeField] private float maximumLockOnDistance;

    private bool isLocked;
    public bool IsLocked { get => isLocked; }

    private PlayerInput input;
    private PlayerInputAction playerInputAction;
    private Camera mainCamera;
    private List<LockableTarget> currentLockedOnTargets = new List<LockableTarget>();

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        input = GetComponent<PlayerInput>();
        playerInputAction = input.PlayerInputAction;

        mainCamera = Camera.main;

        playerInputAction.Player.LockOn.performed += OnLockOn;
        playerInputAction.Player.LockOn.canceled += OnLockOn;
    }

    private void OnDisable()
    {
        playerInputAction.Player.LockOn.performed -= OnLockOn;
        playerInputAction.Player.LockOn.canceled -= OnLockOn;
    }

    private void OnLockOn(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            HandleLockOnTarget();
        }
    }

    private void Update()
    {
        if (isLocked)
        {
            HandleRotatingLockedCamera();
        }
    }

    private void HandleRotatingLockedCamera()
    {
        // rotate followTransform to face the locked on target
        if (currentLockedOnTargets.Count > 0)
        {
            Vector3 directionToTarget = currentLockedOnTargets[0].transform.position - transform.position;
            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            followTransform.rotation = Quaternion.Slerp(followTransform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void HandleLockOnTarget()
    {
        List<LockableTarget> availableLockOnTargets = GetAvailableLockOnTargets();

        // default to lock on the first target
        if (availableLockOnTargets.Count > 0)
        {
            // if the player is already locked on to the target, unlock the camera
            if (currentLockedOnTargets.Count > 0 && availableLockOnTargets[0].transform == currentLockedOnTargets[0].transform)
            {
                cameraScriptableSignalHub.Get<OnPlayerRequestLockOn>().Dispatch(null);
                isLocked = false;
                currentLockedOnTargets.Clear();
                return;
            }

            cameraScriptableSignalHub.Get<OnPlayerRequestLockOn>().Dispatch(availableLockOnTargets[0].transform);

            currentLockedOnTargets = availableLockOnTargets;
            isLocked = true;
        }
        else
        {
            cameraScriptableSignalHub.Get<OnPlayerRequestLockOn>().Dispatch(null);
            isLocked = false;
            currentLockedOnTargets.Clear();
        }
    }

    private List<LockableTarget> GetAvailableLockOnTargets()
    {
        List<LockableTarget> availableLockOnTargets = new List<LockableTarget>();

        // get all colliders detectionRadius around the player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            LockableTarget lockOnTarget = hitColliders[i].GetComponent<LockableTarget>();

            if (lockOnTarget != null)
            {
                Vector3 lockOnTargetDirection = lockOnTarget.RaycastReceiver.position - transform.position;
                Vector3 cameraDirection = followTransform.position - mainCamera.transform.position;

                float distanceFromTarget = Vector3.Distance(lockOnTarget.RaycastReceiver.position, transform.position);

                // viewable angle is the angle between the player's forward direction and the direction to the target
                // change to followTransform.forward maybe
                // since camera is currently following followTransform
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraDirection);

                // TODO: if the target is dead, continue to the next target

                // if accidently locked on to player, continue to the next target
                if (lockOnTarget.transform.root == transform.root)
                {
                    continue;
                }

                // if the target is too far away, continue to the next target
                if (distanceFromTarget > maximumLockOnDistance)
                {
                    continue;
                }

                // if the target is within the viewable angle and within the detection radius
                if (viewableAngle >= -maximumViewableAngle && viewableAngle <= maximumViewableAngle)
                {
                    RaycastHit hit;
                    Debug.DrawRay(raycastTransform.position, lockOnTargetDirection, Color.red, 1f);

                    // if the target hit something before hitting the enemy, continue
                    if (Physics.Linecast(raycastTransform.position, lockOnTargetDirection, out hit, WorldLayerManager.Instance.EnvironmentLayer))
                    {
                        continue;
                    }
                    else
                    {
                        availableLockOnTargets.Add(lockOnTarget);
                    }
                }
            }
        }

        // sort list based on viewable angle
        // very unoptimized, but it's fine for now
        availableLockOnTargets.Sort((a, b) =>
        {
            Vector3 directionToA = a.transform.position - transform.position;
            Vector3 directionToB = b.transform.position - transform.position;

            float angleToA = Vector3.Angle(directionToA, transform.forward);
            float angleToB = Vector3.Angle(directionToB, transform.forward);

            return angleToA.CompareTo(angleToB);
        });

        return availableLockOnTargets;
    }
}
