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

    // rpc 구현
    public bool jumpInput; 
    public bool divingInput;
    public bool grabInput;
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
        InputRot();
        InputMoving();
        InputJump();
        InputDive();
        InputGrab();
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

    //private Queue<bool> jumpBuffer = new Queue<bool>(); // 현재 안씀
    private void InputJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //jumpBuffer.Enqueue(true);
            photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, playerNumber, true);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            //jumpBuffer.Enqueue(false);
            photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, playerNumber, false);
        }
    }

    [PunRPC]
    private void Jump_RPC(int playerNum, bool isJumping)
    {
        if (inputs[playerNum] != null)
        {
            inputs[playerNum].jumpInput = isJumping;
        }
    }

    private void InputDive()
    {
        if(Input.GetButtonDown("Diving"))
        {
            photonView.RPC(nameof(Diving_RPC), RpcTarget.MasterClient, playerNumber, true);
        }
        else if(Input.GetButtonUp("Diving"))
        {
            photonView.RPC(nameof(Diving_RPC), RpcTarget.MasterClient, playerNumber, false);
        }
    }

    [PunRPC]
    private void Diving_RPC(int playerNum, bool isDiving) 
    {
        if (inputs[playerNum] != null)
        {
            inputs[playerNum].divingInput = isDiving;
        }
    }
    
    private void InputGrab()
    {
        if(Input.GetButtonDown("Grab"))
        {
            photonView.RPC(nameof(Grab_RPC), RpcTarget.MasterClient, playerNumber, true);
        }
        else if(Input.GetButtonUp("Grab"))
        {
            photonView.RPC(nameof(Grab_RPC), RpcTarget.MasterClient, playerNumber, false);
        }
    }

    [PunRPC]
    private void Grab_RPC(int playerNum, bool isGrabbing)
    {
        if(inputs[playerNum] != null)
        {
            inputs[playerNum].grabInput = isGrabbing;
        }
    }


    private void InputRot()
    {
        inputs[playerNumber].rotVec.y = Input.GetAxisRaw("Mouse Y");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Util.SendAndReceiveStruct(stream , ref moveDir);
        Util.SendAndReceiveStruct(stream , ref rotVec);
    }
}
