using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayMatch : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] int num;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        SetDescriptionText("Server connecting...");
    }

    private void SetDescriptionText(string msg)
    {
        descriptionText.text = msg;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 진입");
        SetDescriptionText("Wait for Players...");
        PhotonNetwork.LocalPlayer.SetReady(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Room", new RoomOptions { MaxPlayers = 2, IsVisible = false });
        // 2명이 입장상태가 확인되면, 5초?? 더 기다리고 그냥 시작 => 가능??
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.READY))
        {
            bool allReady = CheckAllReady();
            Debug.Log($"모든 플레이어 준비 : {allReady}");
            if (allReady && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                StartCoroutine(WaitPlayerRoutin());
            }
        }
    }

    private bool CheckAllReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady() == false)
            {
                Debug.Log($"{player.GetPlayerNumber()}. {player.GetReady()}");
                return false;
            }
        }

        return true;
    }

    IEnumerator WaitPlayerRoutin()
    {
        SetDescriptionText("Game Loading...");
        yield return new WaitForSeconds(3f);

        if (PhotonNetwork.IsMasterClient == false)
            yield break;

        ChoiceGameScene();
    }

    private void ChoiceGameScene()
    {
        num = Random.Range(0, 3);
        Debug.Log($"당첨된 수 {num}");
        switch (num)
        {
            case 0: // 산 무너져유
                PhotonNetwork.LoadLevel("TestSecne 1.4");
                break;
            case 1: // 바닥 떨어져유
                PhotonNetwork.LoadLevel("Stage2");
                break;
            case 2: // 점프 쇼다운
                PhotonNetwork.LoadLevel("Stage3");
                break;
        }
    }
}
