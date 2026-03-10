using UnityEngine;
using System;

public class BossAnimationController : AnimationControllerBase
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int LoseHash = Animator.StringToHash("Lose");
    private static readonly int DanceHash = Animator.StringToHash("Dance");

    private Action _onHitCallback;

    public void PlayAttack(Action onHit = null)
    {
        _onHitCallback = onHit;
        _animator.ResetTrigger(LoseHash);
        _animator.SetTrigger(AttackHash);
    }

    private void OnHitMoment()
    {
        _onHitCallback?.Invoke();
        _onHitCallback = null;
    }

    public void PlayDance()
    {
        _animator.ResetTrigger(AttackHash);
        _animator.ResetTrigger(LoseHash);
        _animator.SetTrigger(DanceHash);
    }

    public void PlayLose(Action onComplete = null)
    {
        _animator.ResetTrigger(AttackHash);
        StartCoroutine(PlayAndWait(LoseHash, onComplete));
    }
}