using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_NamePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;

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
}
