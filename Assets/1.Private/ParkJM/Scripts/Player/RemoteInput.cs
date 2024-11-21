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
    }

    private void InputMoving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");
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

    private void InputRot()
    {
        rotVec.y = Input.GetAxisRaw("Mouse Y");
    }

    [PunRPC]
    private void Jump_RPC(int playerNum)
    {
        if (inputs[playerNum] != null)
        {
            inputs[playerNum].jumpInput = true;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Util.SendAndReceiveStruct(stream , ref moveDir);
    }
}
