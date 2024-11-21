using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneTester : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";

    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;
        PhotonNetwork.JoinOrCreateRoom(RoomName, options, typedLobby: default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayCoroutine());
    }

    public void TestGameStart()
    {
        Debug.Log("게임 시작");


        if (!PhotonNetwork.IsMasterClient)
            return;
        // ToDo : 방장만 플레이어를 스폰시켜주도록
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate("Player", randomPos, Quaternion.identity);
    }

    IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }
}
