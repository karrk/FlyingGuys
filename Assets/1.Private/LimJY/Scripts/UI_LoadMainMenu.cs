using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadMainMenu : MonoBehaviour
{
    // Result -> MainMenu
    public void MainMenuScene()
    {
        PhotonNetwork.LeaveRoom();
    }
}

