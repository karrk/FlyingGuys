using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScene : MonoBehaviour
{
    [SerializeField] TMP_Text resultText;
    [SerializeField] TMP_Text nicknameText;

    private void Start()
    {
        Init(resultText, nicknameText);
    }

    private void Init(TMP_Text result, TMP_Text nickName)
    {
        nickName.text = PhotonNetwork.LocalPlayer.NickName;

        if (PhotonNetwork.LocalPlayer.GetWinner())
            resultText.text = "Win";
        else
            resultText.text = "Lose";
    }

    public void SceneChange()
    {
        PhotonNetwork.Disconnect();
    }
}
