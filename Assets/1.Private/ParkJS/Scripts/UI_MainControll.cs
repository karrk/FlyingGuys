using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MainControll : MonoBehaviour
{
    [SerializeField] TMP_Text userNameText;
    [SerializeField] TMP_Text playerCount;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject matchPlayer;

    private void Awake()
    {
        userNameText.text = NetWorkManager.NickName;
        panel.SetActive(false);
    }

    public void RandomMatch()
    {
        PhotonNetwork.ConnectUsingSettings();
        panel.SetActive(true);
    }

    private void LateUpdate()
    {
        if (PhotonNetwork.InRoom)
            UpdateCurRoom();
    }

    private void UpdateCurRoom()
    {
        matchPlayer.SetActive(true);
        playerCount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
}
