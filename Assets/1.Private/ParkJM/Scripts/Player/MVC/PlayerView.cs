using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerView : MonoBehaviourPun, IPunObservable
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
    //private void Start()
    //{
    //    if(PhotonNetwork.IsMasterClient)
    //    {
    //        photonView.RPC(nameof(ResetAllTriggers_RPC), RpcTarget.AllBuffered);
    //    }
    //}

    //[PunRPC]
    //private void ResetAllTriggers_RPC()
    //{
    //    foreach (int triggers in animationStateHashes)
    //    {
    //        animator.ResetTrigger(triggers);
    //    }
    //}

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
        Animator.StringToHash("Bouncing"),
        Animator.StringToHash("Pulling"),
        Animator.StringToHash("Pushing"),
        Animator.StringToHash("Struggling"),
    };

    public void BroadCastTriggerParameter(E_AniParameters _parameter)
    {
        photonView.RPC(nameof(SetTriggerParameter_RPC), RpcTarget.All, _parameter);
    }
    public void BroadCastBoolParameter(E_AniParameters _parameter, bool boolValue)
    {
        photonView.RPC(nameof(SetBoolParameter_RPC), RpcTarget.All, _parameter, boolValue);
    }
    public void SetBoolParameter(E_AniParameters _parameter, bool boolValue)
    {
        animator.SetBool(animationStateHashes[(int)_parameter], boolValue);
    }


    [PunRPC]
    public void SetTriggerParameter_RPC(E_AniParameters _parameter)
    {
        animator.SetTrigger(animationStateHashes[(int)_parameter]);
    }

    [PunRPC]
    public void SetBoolParameter_RPC(E_AniParameters _parameter, bool boolValue)
    {
        animator.SetBool(animationStateHashes[(int)_parameter], boolValue);
    }

    public void SetBoolInGrabAnimation(int _animationIndex, bool playing)
    {
        animator.SetBool(grabAnimationHashes[_animationIndex], playing);
    }

    public bool GetBoolParameter(E_AniParameters _parameter)
    {
        return animator.GetBool(animationStateHashes[(int)_parameter]);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    //private int[] animationHashes = new int[]
    //{
    //    Animator.StringToHash("Idle"),
    //    Animator.StringToHash("Run"),
    //    Animator.StringToHash("Jump"),
    //    Animator.StringToHash("Fall"),
    //    Animator.StringToHash("Dive"),
    //    Animator.StringToHash("FallImpact"),
    //    Animator.StringToHash("StandUp"),
    //    Animator.StringToHash("Bounced")
    //};

    //public void PlayAnimation(int _animationIndex)
    //{
    //    if (_animationIndex >= 0 && _animationIndex < animationHashes.Length)
    //    {
    //        animator.Play(animationHashes[_animationIndex], 0, 0);
    //    }
    //    else
    //    {
    //        Debug.LogError("애니메이션 인덱스 에러");
    //    }
    //}

    //public void ToggleRun()
    //{
    //    bool isRunning = animator.GetBool(animationHashes[(int)E_PlayeState.Run]);
    //    animator.SetBool(animationHashes[(int)E_PlayeState.Run], !isRunning);
    //}

    //public void PlayRun()
    //{
    //    animator.SetBool(animationHashes[(int)E_PlayeState.Run], true);
    //}

    //public void StopRun()
    //{
    //    animator.SetBool(animationHashes[(int)E_PlayeState.Run], false);
    //}

    //public void PlayDive()
    //{
    //    animator.SetBool(animationHashes[(int)E_PlayeState.Diving], true);
    //}

    //public void EndDive()
    //{
    //    animator.SetBool(animationHashes[(int)E_PlayeState.Diving], false);
    //}
}
