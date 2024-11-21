using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

[System.Serializable]
public class PlayerObject : MonoBehaviourPun
{
    public Player player;
    [SerializeField] BeforePlayerController playerController;
    [SerializeField] float moveSpeed = 5f;

    private void Start()
    {
        playerController = GetComponent<BeforePlayerController>();
    }

    private void Update()
    {
        //// 룸 오브젝트이므로 방장만 돌릴 수 있음
        //if (!photonView.IsMine) 
        // 이거 마스터 클라이언트로 해야하는거 아닌가
        // 기존 플레이어스포너는 플레이어가 룸 오브젝트가 아니라서 이걸 쓸 수 없었다
        // 플레이어가 룸 오브젝트가 된다면 다시 활성화 할 필요 있음
        //    return;

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 플레이어가 아닙니다.");
            return;
        }

        // 인덱스 에러
        Vector3 inputDir = BeforePlayerController.inputs[player.GetPlayerNumber()].inputDir;
       // Vector3 inputDir = PlayerController.inputs[playerController.playerNumber].inputDir;
        MoveAllPlayer();
    }

    // 이렇게 하는게 아닌것같은데
    private void MoveAllPlayer()
    {
        foreach (BeforePlayerController player in BeforePlayerController.inputs)
        {
            if(player == null)
                continue;

            Vector3 inputDir = player.inputDir.normalized;
            bool isMove = inputDir.sqrMagnitude != 0;

            if (isMove)
            {
                player.rb.velocity = inputDir * moveSpeed;
            }
            else
            {
                player.rb.velocity = Vector3.zero;
            }
        }



        //Debug.Log(player.playerNumber);
        ////Vector3 inputDir = PlayerController.inputs[player.GetPlayerNumber()].inputDir.normalized;
        //Vector3 inputDir = PlayerController.inputs[player.playerNumber].inputDir.normalized;
        //bool isMove = inputDir.sqrMagnitude != 0;
        //if (isMove)
        //{
        //    // PlayerController.inputs[player.GetPlayerNumber()].rb.velocity = inputDir * moveSpeed;
        //    PlayerController.inputs[player.playerNumber].rb.velocity = inputDir * moveSpeed;
        //}

    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    Util.SendAndReceiveStruct(stream, ref inputDir);
    //}
}
