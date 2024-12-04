using Photon.Pun;
using System;
using TMPro;
using UnityEngine;

public class UI_MainControll : MonoBehaviour
{
    [SerializeField] TMP_Text playerCount;
    //[SerializeField] GameObject panel;
    [SerializeField] GameObject matchPlayer;

    private void Awake()
    {
        matchPlayer.SetActive(false);
    }

    private void Start()
    {
        RandomMatch();
    }

    public void RandomMatch()
    {
        if(PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.JoinRandomRoom();

        //PhotonNetwork.ConnectUsingSettings();
        matchPlayer.SetActive(true);
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();

        PhotonNetwork.LocalPlayer.SetColor(ColorPicker.LastColor);
    }

    private void LateUpdate()
    {
        if (PhotonNetwork.InRoom)
            UpdateCurRoom();
    }

    private void UpdateCurRoom()
    {
        playerCount.gameObject.SetActive(true);
        playerCount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
}
