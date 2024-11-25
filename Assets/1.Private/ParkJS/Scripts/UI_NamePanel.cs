using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_NamePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Text playerCount;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject matchPlayer;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void JoinInGame()
    {
        if(nameInput.text =="")
        {
            Debug.LogWarning("닉네임을 정하십시오.");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = nameInput.text;
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.LoadLevel("GameScene");
    }

    public void RandomMatch()
    {
        if (nameInput.text == "")
        {
            Debug.LogWarning("닉네임을 정하십시오.");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = nameInput.text;
        PhotonNetwork.ConnectUsingSettings();
        panel.SetActive(true);
    }

    private void LateUpdate()
    {
        if(PhotonNetwork.InRoom)
            UpdateCurRoom();
    }

    private void UpdateCurRoom()
    {
        matchPlayer.SetActive(true);
        playerCount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
}
