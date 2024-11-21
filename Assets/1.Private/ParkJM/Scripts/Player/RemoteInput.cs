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
    public Vector3 MoveDir { get { return moveDir; } }
    public Vector3 RotVec { get { return rotVec; } }

    public bool jumpInput; // rpc 구현

    private void Awake()
    {
        inputs[photonView.Owner.GetPlayerNumber()] = this;
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
            photonView.RPC(nameof(Jump_RPC), RpcTarget.MasterClient, photonView.Owner.GetPlayerNumber());
        }
    }

    private void InputRot()
    {
        rotVec.y = Input.GetAxisRaw("Mouse Y");
    }

    [PunRPC]
    private void Jump_RPC(int playerNumber)
    {
        if (inputs[playerNumber] != null)
        {
            inputs[playerNumber].jumpInput = true;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Util.SendAndReceiveStruct(stream , ref moveDir);
    }
}
