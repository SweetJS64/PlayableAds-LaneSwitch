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
        _animator.SetTrigger(RunHash);
    }
    
    public void PlayLose()
    {
        ResetAllTriggers();
        _animator.SetTrigger(LoseHash);
    }

    public void PlayDance()
    {
        ResetAllTriggers();
        _animator.SetTrigger(DanceHash);
    }
    
    public void PlayIdle()
    {
        ResetAllTriggers();
        _animator.SetTrigger(IdleHash);
    }
    
    public void PlayAttack(Action onComplete = null)
    {
        ResetAllTriggers();
        StartCoroutine(PlayAndWait(AttackHash, onComplete));
    }

    private void ResetAllTriggers()
    {
        _animator.ResetTrigger(RunHash);
        _animator.ResetTrigger(AttackHash);
        _animator.ResetTrigger(LoseHash);
        _animator.ResetTrigger(DanceHash);
        _animator.ResetTrigger(IdleHash);
    }
}