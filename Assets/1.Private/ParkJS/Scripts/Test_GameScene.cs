using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_GameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] Char_Spawner charSpawner;
    [SerializeField] bool inGamePlay;

    private void Start()
    {
        if(inGamePlay)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutin());
    }

    IEnumerator StartDelayRoutin()
    {
        yield return new WaitForSeconds(1f);
        GameStart();
    }

    private void GameStart()
    {
        Debug.Log("게임 시작");

        // TODO : 모든 클라이언트가 실행 하는 곳
        photonView.RPC(nameof(PlayerSpawn), RpcTarget.MasterClient);

        if (PhotonNetwork.IsMasterClient == false)
            return;

        // TODO : 마스터 클라이언트만 실행 하는 곳
    }

    [PunRPC]
    private void PlayerSpawn(PhotonMessageInfo info)
    {
        // TODO : 플레이어 리모트 스폰 추가

        //charSpawner.SpawnCharacter(info);
    }
}
