using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

public class ResultScene : MonoBehaviour
{
    [SerializeField] TMP_Text resultText;
    [SerializeField] TMP_Text nicknameText;

    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem vfx;
    [SerializeField] GameObject trophy;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        Init(resultText, nicknameText);
    }

    private void Init(TMP_Text result, TMP_Text nickName)
    {
        int number = PhotonNetwork.LocalPlayer.GetPlayerNumber();

        if (NetWorkManager.PlayerResults[number, 0] == false)
            return;

        if(nickName != null)
            nickName.text = PhotonNetwork.LocalPlayer.NickName;

        bool winResult = NetWorkManager.PlayerResults[number,1];

        if (winResult == true)
        {
            trophy.SetActive(true);
            anim.SetBool("Win", true);
            vfx.Play();
            resultText.text = "Win";
        }
        else
            resultText.text = "Lose";

        NetWorkManager.PlayerResults[number, 0] = false;
        NetWorkManager.PlayerResults[number, 1] = false;
    }
}
