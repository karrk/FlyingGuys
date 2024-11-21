using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;


public class PlayerController : MonoBehaviourPun, IPunObservable
{
    
    public static PlayerController[] inputs = new PlayerController[8]; // 임시 최대인원수
    public Player player;
    public Vector3 inputDir;
    public Rigidbody rb;
    public int playerNumber;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (!photonView.IsMine)
            return;

        player = PhotonNetwork.LocalPlayer;
        playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
    }


    private void Start()
    {
        if (!photonView.IsMine)
            return;

        //player = PhotonNetwork.LocalPlayer;
        //int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        Debug.Log($"플레이어 넘버 :  {player.GetPlayerNumber()}");
        if (playerNumber >= 0 && playerNumber < inputs.Length)
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

        //if(Input.GetKeyDown("Jump"))

        if(Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC(nameof(Jump), RpcTarget.MasterClient);
        }

        //photonView.RPC(nameof(SentMoveInputToMaster), RpcTarget.MasterClient, inputDir);
    }

    private void HandleInputs()
    {
        inputDir.x = Input.GetAxisRaw("Horizontal");
        inputDir.z = Input.GetAxisRaw("Vertical");
    }

    //[PunRPC]
    //private void SentMoveInputToMaster(Vector3 _inputDir)
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //        return;

    //    PlayerController.inputs[playerNumber].inputDir = _inputDir;
    //}

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

