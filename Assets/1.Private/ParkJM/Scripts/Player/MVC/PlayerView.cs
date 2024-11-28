using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerView : MonoBehaviourPun
{
    PlayerController player;
    [HideInInspector] public AnimatorStateInfo stateInfo;
    private Animator animator;
    private Transform playerChestTr;
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;
    [SerializeField] float offsetZ;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        playerChestTr = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        offsetX = 0;
        offsetY = 0;
        offsetZ = 42.5f;
    }

    private int[] animationHashes = new int[]
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Run"),
        Animator.StringToHash("Jump"),
        Animator.StringToHash("Fall"),
        Animator.StringToHash("Dive"),
        Animator.StringToHash("FallImpact"),
        Animator.StringToHash("StandUp"),
        Animator.StringToHash("Bounced")
    };

    private int[] grabAnimationHashes = new int[]
    {
        Animator.StringToHash("Pushing"),
        Animator.StringToHash("Pulling"),
        Animator.StringToHash("Struggling"),
    };

    public void UpSpine()
    {
        
        Vector3 chestDir = player.camTransform.forward;
        playerChestTr.LookAt(chestDir);
        playerChestTr.localRotation = Quaternion.Euler(offsetX, offsetY, offsetZ);

    }

    private int[] animationStateHashes = new int[]
    {
        Animator.StringToHash("Idling"),
        Animator.StringToHash("Running"),
        Animator.StringToHash("Jumping"),
        Animator.StringToHash("Falling"),
        Animator.StringToHash("Diving"),
        Animator.StringToHash("FallingImpact"),
        Animator.StringToHash("StandingUp"),
        Animator.StringToHash("Bouncing")
    };

    public void SetTriggerParameter(E_AniParameters _parameter)
    {
        animator.SetTrigger(animationStateHashes[(int)_parameter]);
    }

    //public void SetAnimationTrigger(string tirggerName)
    //{
    //    animator.SetTrigger(tirggerName);
    //}

    public void SetBoolParameter(E_AniParameters _parameter, bool _bool)
    {
        animator.SetBool(animationStateHashes[(int)_parameter], _bool);
    }


    public void PlayAnimation(int _animationIndex)
    {
        if (_animationIndex >= 0 && _animationIndex < animationHashes.Length)
        {
            animator.Play(animationHashes[_animationIndex], 0, 0);
        }
        else
        {
            Debug.LogError("애니메이션 인덱스 에러");
        }
    }

    public void SetBoolInGrabAnimation(int _animationIndex, bool playing)
    {
        animator.SetBool(grabAnimationHashes[_animationIndex], playing);
    }

    public bool GetBoolInGrabAnimation(int _animationIndex)
    {
        return animator.GetBool(grabAnimationHashes[_animationIndex]);
    }


    public void ToggleRun()
    {
        bool isRunning = animator.GetBool(animationHashes[(int)E_PlayeState.Run]);
        animator.SetBool(animationHashes[(int)E_PlayeState.Run], !isRunning);
    }

    public void PlayRun()
    {
        animator.SetBool(animationHashes[(int)E_PlayeState.Run], true);
    }

    public void StopRun()
    {
        animator.SetBool(animationHashes[(int)E_PlayeState.Run], false);
    }

    public void PlayDive()
    {
        animator.SetBool(animationHashes[(int)E_PlayeState.Diving], true);
    }

    public void EndDive()
    {
        animator.SetBool(animationHashes[(int)E_PlayeState.Diving], false);
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
