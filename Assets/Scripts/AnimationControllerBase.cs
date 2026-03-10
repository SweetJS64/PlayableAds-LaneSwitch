using UnityEngine;
using System;
using System.Collections;

public abstract class AnimationControllerBase : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    protected IEnumerator PlayAndWait(int triggerHash, Action onComplete)
    {
        _animator.SetTrigger(triggerHash);

        yield return null;

        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        onComplete?.Invoke();
    }
}
