using UnityEngine;
using System;
using System.Collections;

public abstract class AnimationControllerBase : MonoBehaviour
{
    [SerializeField] protected Animator Animator;

    protected IEnumerator PlayAndWait(int triggerHash, Action onComplete)
    {
        Animator.SetTrigger(triggerHash);

        var stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        onComplete?.Invoke();
    }
}
