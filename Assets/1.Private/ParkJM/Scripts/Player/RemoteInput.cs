using JetBrains.Annotations;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteInput : MonoBehaviourPun, IPunObservable
{
    public static RemoteInput[] inputs = new RemoteInput[8];

    //PlayerController player;
    [SerializeField] Vector3 moveDir;
    [SerializeField] Vector3 rotVec;
    [SerializeField] int playerNumber;
    public Vector3 MoveDir { get { return moveDir; } }
    public Vector3 RotVec { get { return rotVec; } }

    public bool jumpInput; // rpc 구현
    public bool divingInput;

    Vector3 moveInput;

    private Vector3 camForward;
    private Vector3 camRight;

    private void Awake()
    {
        playerNumber = photonView.Owner.GetPlayerNumber();
        inputs[playerNumber] = this;
        
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        InputMoving();
        InputJump();
        InputRot();
        InputDiving();
    }

    private void InputMoving()
    {
        camForward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

        moveDir = (moveInput.z * camForward + moveInput.x * camRight).normalized;
    }

    private bool preJumpInput = false;
    private void InputJump()
    {
        bool curJumpInput = Input.GetButtonDown("Jump");

        if(curJumpInput != preJumpInput)
        {
            photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, playerNumber, curJumpInput);
            preJumpInput = curJumpInput;
        }




        //if (curJumpInput != preJumpInput)
        //{
        //    //photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, photonView.Owner.GetPlayerNumber());
        //    photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, playerNumber, true);
        //}
        //else
        //{
        //    photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, playerNumber, false);
        //    //inputs[playerNumber].jumpInput = false;
        //}
    }

    [PunRPC]
    private void Jump_RPC(int playerNum, bool isJumping)
    {
        //if (!PhotonNetwork.IsMasterClient)
        //    return;

        if (inputs[playerNum] != null)
        {
            // 지금 remoteInput 객체는 룸 오브젝트가 아니라 각 유저가 소유권을 가지는데 
            // 아래 로그가 마스터의 콘솔창에서만 출력됨
            // rpc를 마스터클라이언트에만 보내니까 그럼
            inputs[playerNum].jumpInput = isJumping;
            Debug.Log("점프 input 들어감");
        }
    }

    private void InputDiving()
    {
        if(Input.GetButtonDown("Diving"))
        {
            photonView.RPC(nameof(Diving_RPC), RpcTarget.MasterClient, playerNumber);
        }
        else
        {
            inputs[playerNumber].divingInput = false;
        }
    }

    private void InputRot()
    {
        inputs[playerNumber].rotVec.y = Input.GetAxisRaw("Mouse Y");
    }

    // Rpc 메서드들



    [PunRPC]
    private void Diving_RPC(int playerNum)
    {
        if (inputs[playerNum] != null)
        {
            inputs[playerNum].divingInput = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Util.SendAndReceiveStruct(stream , ref moveDir);
        Util.SendAndReceiveStruct(stream , ref rotVec);
    }
}
