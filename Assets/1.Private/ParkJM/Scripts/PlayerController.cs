using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;


public class PlayerController : MonoBehaviourPun, IPunObservable
{
    
    public static PlayerController[] inputs = new PlayerController[8];
    public Player player;
    public Vector3 inputDir;
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        player = PhotonNetwork.LocalPlayer;
        int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        
        if(playerNumber >= 0 && playerNumber < inputs.Length)
        {
            inputs[playerNumber] = this;
        }
        else
        {
            Debug.LogError($"플레이어 넘버 인덱스 오류: {playerNumber}");
        }
        
    }

    private void Update()
    {
        if(!photonView.IsMine)
            return;

        HandleInputs();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC(nameof(Jump), RpcTarget.MasterClient);
        }
    }

    private void HandleInputs()
    {
        inputDir.x = Input.GetAxisRaw("Horizontal");
        inputDir.z = Input.GetAxisRaw("Vertical");
    }

    [PunRPC]
    private void Jump()
    {
        Debug.Log("점프함");
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Util.SendAndReceiveStruct(stream, ref inputDir);
    }

    
}

// 따로 둘걸 상정
public class PlayerObject : MonoBehaviourPun
{
    public Player player;
    [SerializeField] float moveSpeed = 5f;

    private void Update()
    {
        // 룸 오브젝트이므로 방장만 돌릴 수 있음
        if (!photonView.IsMine) 
            return;

        //Vector3 inputDir = PlayerController.inputs[player.GetPlayerNumber()].inputDir;


    }

    private void MovePlayer()
    {

        Vector3 inputDir = PlayerController.inputs[player.GetPlayerNumber()].inputDir.normalized;
        bool isMove = inputDir.sqrMagnitude != 0;
        if(isMove)
        {
            PlayerController.inputs[player.GetPlayerNumber()].rb.velocity = inputDir * moveSpeed;
        }

    }



}