using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
        PhotonNetwork.AutomaticallySyncScene = true;
        RandomMatch();
    }

    public void RandomMatch()
    {
        PhotonNetwork.ConnectUsingSettings();
        matchPlayer.SetActive(true);
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
