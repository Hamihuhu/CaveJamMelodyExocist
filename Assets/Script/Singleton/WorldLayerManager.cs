using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayerManager : MonoBehaviour
{
    public static WorldLayerManager Instance;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask environmentLayer;

    public LayerMask PlayerLayer { get => playerLayer; }
    public LayerMask EnemyLayer { get => enemyLayer; }
    public LayerMask EnvironmentLayer { get => environmentLayer; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
