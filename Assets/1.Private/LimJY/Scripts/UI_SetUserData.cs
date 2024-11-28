using Photon.Pun;
using TMPro;
using UnityEngine;

public class UI_SetUserData : MonoBehaviour
{
    [SerializeField] TMP_Text userNameText;

    private void Start()
    {
        userNameText.text = PhotonNetwork.LocalPlayer.NickName;
    }
}
