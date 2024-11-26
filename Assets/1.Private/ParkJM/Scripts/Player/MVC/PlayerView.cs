using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviourPun
{
    [HideInInspector] public AnimatorStateInfo stateInfo;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private int[] animationHash = new int[]
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Running"),
        Animator.StringToHash("JumpUp"),
        Animator.StringToHash("Fall"),
        Animator.StringToHash("Dive"),
        Animator.StringToHash("Bounced"),
    };

    public void PlayAnimation(int _animationIndex)
    {
        if (_animationIndex >= 0 && _animationIndex < animationHash.Length)
        {
            animator.Play(animationHash[_animationIndex], 0, 0);
        }
        else
        {
            Debug.LogError("애니메이션 인덱스 에러");
        }
    }

    public void PlayerAnimation(E_PlayeState state)
    {
        animator.SetTrigger(animationHash[(int)state]);
    }

    public void PlayRun()
    {
        animator.SetBool(animationHash[(int)E_PlayeState.Run], true);
    }

    public void StopRun()
    {
        animator.SetBool(animationHash[(int)E_PlayeState.Run], false);
    }

    public void PlayDive()
    {
        animator.SetBool(animationHash[(int)E_PlayeState.Diving], true);
    }

    public void EndDive()
    {
        animator.SetBool(animationHash[(int)E_PlayeState.Diving], false);
    }

    public bool IsAnimationFinished()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f)
        {
            return true;
        }
        else
            return false;
    }

}
