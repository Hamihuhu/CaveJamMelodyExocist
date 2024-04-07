using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushableTargetDetection : MonoBehaviour
{
    [SerializeField] private ScriptableSignalHub rushScriptableSignalHub;
    [SerializeField] private Transform followTransform;
    [SerializeField] private Transform raycastTransform;
    [SerializeField] private float maximumRushAngle;
    [SerializeField] private float maximumRushDistance;
    [SerializeField] private float stopRushDistance;
    [SerializeField] private float detectionRadius;

    private PlayerInput input;
    private PlayerInputAction playerInputAction;
    private Camera mainCamera;

    private List<RushableTarget> availableRushableTargets;
    private Tweener rushTween;

    private void Start()
    {
        mainCamera = Camera.main;

        input = GetComponent<PlayerInput>();
        playerInputAction = input.PlayerInputAction;
        
        playerInputAction.Player.Rush.performed += OnRush;
        playerInputAction.Player.Rush.canceled += OnRush;
    }

    private void OnEnable()
    {
        if (playerInputAction == null)
        {
            return;
        }
        playerInputAction.Player.Rush.performed += OnRush;
        playerInputAction.Player.Rush.canceled += OnRush;
    }

    private void OnDisable()
    {
        playerInputAction.Player.Rush.performed -= OnRush;
        playerInputAction.Player.Rush.canceled -= OnRush;
    }

    private void Update()
    {
        HandleGetAvailableRushableTarget();
    }

    private void OnRush(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            HandleRushTowardsTarget();
        }
    }

    private void HandleGetAvailableRushableTarget()
    {
        availableRushableTargets = GetAvailableRushableTarget();
        rushScriptableSignalHub.Get<OnPlayerRequestRush>().Dispatch(availableRushableTargets);
    }

    private void HandleRushTowardsTarget()
    {
        // if there are available rushable targets
        // default rush towards the first target that is closest to the middle of FOV
        if (availableRushableTargets.Count > 0 && rushTween == null)
        {
            Vector3 rushDirection = availableRushableTargets[0].transform.position - transform.position;

            // limit the distance the player can rush, stoping the rush at stopRushDistance from the target
            Vector3 rushTargetPosition = availableRushableTargets[0].transform.position - rushDirection.normalized * stopRushDistance;

            transform.DOMove(rushTargetPosition, 0.5f)
                .SetEase(Ease.Linear)
                .OnComplete(() => rushTween = null);
        }
    }

    private List<RushableTarget> GetAvailableRushableTarget()
    {
        List<RushableTarget> availableRushableTargets = new List<RushableTarget>();

        // get all colliders detectionRadius around the player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            RushableTarget rushableTarget = hitColliders[i].GetComponent<RushableTarget>();

            if (rushableTarget != null)
            {
                Vector3 rushableTargetDirection = rushableTarget.RaycastReceiver.position - transform.position;
                Vector3 cameraDirection = followTransform.position - mainCamera.transform.position;

                float distanceFromTarget = Vector3.Distance(rushableTarget.RaycastReceiver.position, transform.position);

                // viewable angle is the angle between the player's forward direction and the direction to the target
                // change to followTransform.forward maybe
                // since camera is currently following followTransform
                float viewableAngle = Vector3.Angle(rushableTargetDirection, cameraDirection);

                // TODO: if the target is dead, continue to the next target

                // if accidently locked on to player, continue to the next target
                if (rushableTarget.transform.root == transform.root)
                {
                    continue;
                }

                // if the target is too far away, continue to the next target
                if (distanceFromTarget > maximumRushDistance)
                {
                    continue;
                }

                // if the target is within the viewable angle and within the detection radius
                if (viewableAngle >= -maximumRushAngle && viewableAngle <= maximumRushAngle)
                {
                    RaycastHit hit;
                    Debug.DrawRay(raycastTransform.position, rushableTargetDirection, Color.red, 1f);

                    // if the target hit something before hitting the enemy, continue
                    if (Physics.Linecast(raycastTransform.position, rushableTargetDirection, out hit, WorldLayerManager.Instance.EnvironmentLayer))
                    {
                        continue;
                    }
                    else
                    {
                        availableRushableTargets.Add(rushableTarget);
                    }
                }
            }
        }

        // sort list based on viewable angle
        // very unoptimized, but it's fine for now
        availableRushableTargets.Sort((a, b) =>
        {
            Vector3 directionToA = a.transform.position - transform.position;
            Vector3 directionToB = b.transform.position - transform.position;

            float angleToA = Vector3.Angle(directionToA, transform.forward);
            float angleToB = Vector3.Angle(directionToB, transform.forward);

            return angleToA.CompareTo(angleToB);
        });

        return availableRushableTargets;
    }
}
