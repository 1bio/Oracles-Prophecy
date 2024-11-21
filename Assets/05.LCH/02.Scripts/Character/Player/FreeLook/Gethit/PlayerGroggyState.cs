using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroggyState : PlayerBaseState
{
    public readonly int GetHit = Animator.StringToHash("GetHit"); // 블렌드 트리

    public readonly int Impact = Animator.StringToHash("Impact"); // 애니메이션 해쉬

    public readonly float CrossFadeDuration = 0.1f;

    public readonly float DampTime = 0.1f;

    public PlayerGroggyState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }


    #region abstarct Methods
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(GetHit, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Animator.SetFloat(Impact, 3f, DampTime, deltaTime);

        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        /*// 애니메이션 재생이 끝난 후
        if (currentInfo.normalizedTime >= 0.8f && stateMachine.WeaponPrefabs[0].activeSelf)
        {
            stateMachine.ChangeState(new PlayerFreeLookState(stateMachine));
            return;
        }
        else if (currentInfo.normalizedTime >= 0.8f && stateMachine.WeaponPrefabs[1].activeSelf)
        {
            stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
            return;
        }*/
    }

    public override void Exit()
    {
    }
    #endregion
}

