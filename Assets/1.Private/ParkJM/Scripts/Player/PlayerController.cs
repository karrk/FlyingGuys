using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 moveDir;
    
    private Player player;
    [SerializeField] int playerNumber;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = (Player)photonView.InstantiationData[0];
        playerNumber = player.GetPlayerNumber();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (RemoteInput.inputs[playerNumber].jumpInput)
        {
            Debug.Log("점프 입력됨");
            JumpTemp();
            RemoteInput.inputs[playerNumber].jumpInput = false;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        moveDir = RemoteInput.inputs[playerNumber].MoveDir;
        rb.velocity = moveDir.normalized * moveSpeed;
    }

    private void JumpTemp()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
