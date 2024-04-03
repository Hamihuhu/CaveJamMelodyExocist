using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AttackSO",menuName ="ScriptableObject/AttackSO",order =1)]
public class ComboSO : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public float modify;
}
