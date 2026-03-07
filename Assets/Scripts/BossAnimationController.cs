using UnityEngine;
using System;
using System.Collections;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator Animator;

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
        StartCoroutine(PlayAndWait(LoseHash, onComplete));
    }

    private IEnumerator PlayAndWait(int triggerHash, Action onComplete)
    {
        Animator.ResetTrigger(AttackHash);
        Animator.SetTrigger(triggerHash);

        var stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        onComplete?.Invoke();
    }
}