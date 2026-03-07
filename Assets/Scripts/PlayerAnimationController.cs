using UnityEngine;
using System;

public class PlayerAnimationController : AnimationControllerBase
{

    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int LoseHash = Animator.StringToHash("Lose");
    private static readonly int DanceHash = Animator.StringToHash("Dance");
    private static readonly int IdleHash = Animator.StringToHash("Idle");

    public void PlayRun()
    {
        ResetAllTriggers();
        Animator.SetTrigger(RunHash);
    }
    
    public void PlayLose()
    {
        ResetAllTriggers();
        Animator.SetTrigger(LoseHash);
    }

    public void PlayDance()
    {
        ResetAllTriggers();
        Animator.SetTrigger(DanceHash);
    }
    
    public void PlayIdle()
    {
        ResetAllTriggers();
        Animator.SetTrigger(IdleHash);
    }
    
    public void PlayAttack(Action onComplete = null)
    {
        ResetAllTriggers();
        StartCoroutine(PlayAndWait(AttackHash, onComplete));
    }

    private void ResetAllTriggers()
    {
        Animator.ResetTrigger(RunHash);
        Animator.ResetTrigger(AttackHash);
        Animator.ResetTrigger(LoseHash);
        Animator.ResetTrigger(DanceHash);
        Animator.ResetTrigger(IdleHash);
    }
}