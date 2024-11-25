using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayMatch : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text descriptionText;

    private void Start()
    {
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
        StartCoroutine(WaitPlayerRoutin());
    }

    IEnumerator WaitPlayerRoutin()
    {
        while (true)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                Debug.Log("충족함");
                SetDescriptionText("Game Loading...");
                yield return new WaitForSeconds(3f);
                PhotonNetwork.LoadLevel("GameScene");
                yield break;
            }
            yield return null;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Room", new RoomOptions { MaxPlayers = 2, IsVisible = false });
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.READY))
        {
            Debug.Log($"{targetPlayer.GetPlayerNumber()} 번 플레이 로딩 완료");
            bool allReady = CheckAllReady();
            Debug.Log($"모든 플레이어 준비 : {allReady}");
            if (allReady)
            {
                Debug.Log("모든 플레이어 준비 완료");
                // TODO : Player All Ready... 
            }
        }
    }

    private bool CheckAllReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)
            {
                Debug.Log($"1. {player.GetLoad() == false}");
                return false;
            }
        }

        return true;
    }
}
