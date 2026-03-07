using UnityEngine;
using System;
using System.Collections;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int LoseHash = Animator.StringToHash("Lose");
    private static readonly int DanceHash = Animator.StringToHash("Dance");

    public void PlayAttack()
    {
        Animator.ResetTrigger(LoseHash);
        Animator.ResetTrigger(DanceHash);
        Animator.SetTrigger(AttackHash);
    }

    public void PlayLose(Action onComplete = null)
    {
        StartCoroutine(PlayAndWait(LoseHash, onComplete));
    }

    private IEnumerator PlayAndWait(int triggerHash, Action onComplete)
    {
        Animator.ResetTrigger(AttackHash);
        Animator.ResetTrigger(DanceHash);
        Animator.SetTrigger(triggerHash);

        var stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        onComplete?.Invoke();
    }

    public void PlayDance()
    {
        Animator.ResetTrigger(AttackHash);
        Animator.ResetTrigger(LoseHash);
        Animator.SetTrigger(DanceHash);
    }
}