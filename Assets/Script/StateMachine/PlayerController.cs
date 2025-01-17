
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading;
using UnityEngine;


public partial class PlayerController : PlayerStateMachine
{
    public List<ComboSO> combos;
    float lastClicked;
    float lastComboEnd;
    private bool CanAttack;
    private bool _isAttackPressed;
    [SerializeField] private TrailRenderer _attackTrail;
  
    private int comboCounter;
    private List<ActionItem> inputBuffer;
    private bool canAllowBuffer;
    public int count;
    private bool IsHoldAttack => input.Hold;

    protected bool IsNormalAttack => input.Tap;
    protected override void SetVariables()
    {
        CanAttack = true;
        canAllowBuffer = true;
        comboCounter = 0;
        _attackTrail.gameObject.SetActive(false);
        inputBuffer = new List<ActionItem>();

        base.SetVariables();
    }
    protected override void Update()
    {
        HandleAttack();
        count = inputBuffer.Count;
        if (canAllowBuffer)
        {
            TryBufferedAction();
        }
        base.Update();
    }
    private void TryBufferedAction()
    {

        if (inputBuffer.Count > 0)
        {
            foreach (ActionItem ai in inputBuffer.ToArray())
            {
                inputBuffer.Remove(ai);
                if (ai.CheckIfValid())
                {

                    DoAction(ai);
                    break;
                }
            }

        }
    }

    private void DoAction(ActionItem item)
    {
        NormalAttack();
        canAllowBuffer = false;
        Invoke(nameof(ReleaseAllowBuffer),item.timeToNextBuffer);
    }

    private void ReleaseAllowBuffer()
    {
        canAllowBuffer = true;
    }

    private void HandleAttack()
    {
        AttackEndProceed();

        if (IsNormalAttack)
        {
            TapHandle();
        }
        else HoldHandle();

    }

    private void TapHandle()
    {
        input.Tap = false;
        inputBuffer.Add(new ActionItem(ActionItem.ActionInput.NormalAttack, Time.time, 1));
    }

    private void HoldHandle()
    {
        if (IsHoldAttack)
        {
            animator.SetBool(IDHoldAttack, true);
        }
        else
        {
            animator.SetBool(IDHoldAttack, false);
        }
    }

    private void AttackEndProceed()
    {
        if (animator.IsTag("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        {
            AttackEnd();
            Invoke(nameof(ComboEnd), 1);
            TryBufferedAction();
        }
    }

    private void AttackEnd()
    {

        _attackTrail.gameObject.SetActive(false);
        CanAttack = true;
        CanMove = true;
        CanRotation = true;

    }
    private void ComboEnd()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
  
    private void NormalAttack()
    {
        if (!CanAttack) return;

        if (comboCounter < combos.Count)
        {
            if (IsTimeToAttack())
            {
                StartCombo();
            }
        }
    }

    private bool IsTimeToAttack()
    {
        return Time.time - lastClicked >= 0.2f;
    }

    private void StartCombo()
    {
        CancelInvoke(nameof(ComboEnd));
        _attackTrail.gameObject.SetActive(true);
        CanAttack = false;
        CanRotation = false;
        input.Tap = false;
        CanMove = false;
        _isAttackPressed = false;
        animator.runtimeAnimatorController = combos[comboCounter].animatorOV;
        comboCounter++;
        lastClicked = Time.time;
        animator.Play("MeleeCombo", 0, 0);
        if (comboCounter >= combos.Count) comboCounter = 0;
    }

    public override void ReleaseAction()
    {

        input.Tap = false; 
        AttackEnd();

        ComboEnd();
    }

    public void CauseDamage(GameObject obj,HitType type)
    {
    
        GetComponent<HitStop>().TriggerHitStop();
  
        obj.GetComponent<Enemy>().TakeDame(type,transform.position);
    }
}
