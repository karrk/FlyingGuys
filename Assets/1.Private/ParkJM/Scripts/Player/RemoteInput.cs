using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteInput : MonoBehaviourPun, IPunObservable
{
    public static RemoteInput[] inputs = new RemoteInput[8];

    [SerializeField] Vector3 moveDir;
    public Vector3 MoveDir { get { return moveDir; } }

    [SerializeField] bool jumpInput; // rpc 구현

    private void Awake()
    {
        inputs[photonView.Owner.GetPlayerNumber()] = this;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Util.SendAndReceiveStruct(stream , ref moveDir);
    }
}
