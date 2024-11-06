using UnityEditor;
using UnityEngine;

public class MonsterAnimationController
{
    public MonsterAnimationController(Animator animator, ObjectFadeInOut objectFadeInOut, float blendTransitionSpeed)
    {
        Animator = animator;
        ObjectFadeInOut = objectFadeInOut;
        BlendTransitionSpeed = blendTransitionSpeed;
        LocomotionBlendValue = 0f;
        CurrentAnimationName = string.Empty;
    }

    public Animator Animator { get; private set; }
    private string CurrentAnimationName { get; set; }
    public ObjectFadeInOut ObjectFadeInOut { get; }
    public AnimatorStateInfo AnimatorStateInfo { get; set; }

    public bool IsLockedInAnimation { get; set; } = false;
    public float LocomotionBlendValue { get; private set; }
    public float BlendTransitionSpeed { get; private set; }

    public void SetLocomotion(float targetBlendValue)
    {
        LocomotionBlendValue = Mathf.Lerp(LocomotionBlendValue, targetBlendValue, BlendTransitionSpeed * Time.deltaTime);

        if (Mathf.Abs(LocomotionBlendValue - targetBlendValue) <= 0.1f)
        {
            LocomotionBlendValue = targetBlendValue;
        }

        Animator.SetFloat("Locomotion", LocomotionBlendValue);
    }

    public void PlayIdleAnimation()
    {
        SetLocomotion(0);
    }

    public void PlayWalkAnimation()
    {
        SetLocomotion(1);
    }

    public void PlayAttackAnimation(int attackCount)
    {
        IsLockedInAnimation = true;
        int randNum = Random.Range(1, attackCount + 1);
        CurrentAnimationName = $"Attack{randNum}";
        Animator.SetTrigger(CurrentAnimationName);
    }

    public void PlaySpawnAnimation()
    {
        IsLockedInAnimation = true;
        CurrentAnimationName = "Spawn";
        Animator.SetTrigger(CurrentAnimationName);
    }

    public void PlayGotHitAnimation()
    {
        IsLockedInAnimation = true;
        CurrentAnimationName = "GotHit";
        Animator.SetTrigger(CurrentAnimationName);
    }

    public void PlayDeathAnimation()
    {
        IsLockedInAnimation = true;
        CurrentAnimationName = "Death";
        Animator.SetTrigger(CurrentAnimationName);
    }

    public void PlaySkillAnimation(string animationName)
    {
        IsLockedInAnimation = true;
        CurrentAnimationName = animationName;
        Animator.SetTrigger(CurrentAnimationName);
    }

    public bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        return AnimatorStateInfo.IsName(animationName);
    }
}
