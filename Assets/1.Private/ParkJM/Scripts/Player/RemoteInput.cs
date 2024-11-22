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

    private void InputJump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            //photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, photonView.Owner.GetPlayerNumber());
            photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, playerNumber);
        }
        else
        {
            inputs[playerNumber].jumpInput = false;
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
    private void Jump_RPC(int playerNum)
    {
        if (inputs[playerNum] != null)
        {
            inputs[playerNum].jumpInput = true;
        }
    }

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
