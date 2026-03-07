using UnityEngine;
using System;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    
    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int LoseHash = Animator.StringToHash("Lose");
    private static readonly int DanceHash = Animator.StringToHash("Dance");

    public void PlayRun()
    {
        Animator.ResetTrigger(AttackHash);
        Animator.ResetTrigger(LoseHash);
        Animator.ResetTrigger(DanceHash);
        Animator.SetTrigger(RunHash);
    }
    
    public void PlayAttack(Action onComplete = null)
    {
        StartCoroutine(PlayAndWait(AttackHash, onComplete));
    }

    private IEnumerator PlayAndWait(int triggerHash, System.Action onComplete)
    {
        Animator.ResetTrigger(RunHash);
        Animator.ResetTrigger(LoseHash);
        Animator.ResetTrigger(DanceHash);
        Animator.SetTrigger(triggerHash);
    
        var stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
    
        onComplete?.Invoke();
    }

    public void PlayLose()
    {
        Animator.ResetTrigger(RunHash);
        Animator.ResetTrigger(AttackHash);
        Animator.ResetTrigger(DanceHash);
        Animator.SetTrigger(LoseHash);
    }

    public void PlayDance()
    {
        Animator.ResetTrigger(RunHash);
        Animator.ResetTrigger(AttackHash);
        Animator.ResetTrigger(LoseHash);
        Animator.SetTrigger(DanceHash);
    }
}