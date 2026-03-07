using UnityEngine;
using System;

public class BossAnimationController : AnimationControllerBase
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int LoseHash = Animator.StringToHash("Lose");

    private Action _onHitCallback;

    public void PlayAttack(Action onHit = null)
    {
        _onHitCallback = onHit;
        Animator.ResetTrigger(LoseHash);
        Animator.SetTrigger(AttackHash);
    }

    private void OnHitMoment()
    {
        _onHitCallback?.Invoke();
        _onHitCallback = null;
    }

    public void PlayLose(Action onComplete = null)
    {
        Animator.ResetTrigger(AttackHash);
        StartCoroutine(PlayAndWait(LoseHash, onComplete));
    }
}