using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_GameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] Char_Spawner charSpawner;

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

        // TODO : 마스터 클라이언트가 실행해야 하는 곳
        PlayerSpawn();

        if (PhotonNetwork.IsMasterClient == false)
            return;

        // TODO : 개인이 실행해야 하는 곳
    }

    private void PlayerSpawn()
    {
        charSpawner.Spawn(PhotonNetwork.LocalPlayer.GetPlayerNumber());
    }
}
