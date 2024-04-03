using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EffectManager : MonoBehaviour
{
    [SerializeField] PhysicsDetection  NA_Detection;
    [SerializeField] PhysicsDetection HA_Detection;
    [SerializeField] PlayerController playerController;
    [SerializeField] private ObjectPooler<Reference> _poolHit;
    [SerializeField] private Transform slotsVFX;
    [SerializeField] private Reference hitPrefab;

    private void Awake()
    {
        NA_Detection.CollisionEnterEvent.AddListener((x) => CauseDamage(x));
        NA_Detection.PositionEnterEvent.AddListener(x=>_poolHit.Get(Helper.RandomPosition(x,-0.3f,0.3f)));

        HA_Detection.CollisionEnterEvent.AddListener((x)=>CauseDamageHold(x));  
        HA_Detection.PositionEnterEvent.AddListener((x)=>_poolHit.Get(x));  

    }
    private void Start()
    {
        Initialization();
    }
    private void Initialization()
    {
        _poolHit = new ObjectPooler<Reference>(hitPrefab, slotsVFX, 5);
    }
    public void CheckNADetection()=>NA_Detection?.CheckCollision(); 
    public void CheckHADetection()=>HA_Detection?.CheckCollision(); 
    public void CauseDamage(GameObject obj) => playerController.CauseDamage(obj,HitType.Tap);
    public void CauseDamageHold(GameObject obj) => playerController.CauseDamage(obj, HitType.Hold);

}
