using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadMainMenu : MonoBehaviour
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        if (PhotonNetwork.InRoom == true)
            PhotonNetwork.LeaveRoom();
    }

    public void MainMenuScene()
    {
        PhotonNetwork.LoadLevel("Public_Menu");
    }
}

