using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayMatch : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text descriptionText;

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
        PhotonNetwork.LoadLevel("TestSecne 1.4");
    }
}
