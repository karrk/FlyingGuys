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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PhotonNetwork.LeaveRoom();
    }

    public void RandomMatch()
    {
        if(PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.JoinRandomRoom();

        matchPlayer.SetActive(true);
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
