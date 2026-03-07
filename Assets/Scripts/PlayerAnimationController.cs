using UnityEngine;

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
    
    public void PlayAttack()
    {
        Animator.ResetTrigger(RunHash);
        Animator.ResetTrigger(LoseHash);
        Animator.ResetTrigger(DanceHash);
        Animator.SetTrigger(AttackHash);
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