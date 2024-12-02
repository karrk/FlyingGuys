using Photon.Pun;
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
        if(nickName != null)
            nickName.text = PhotonNetwork.LocalPlayer.NickName;

        if (PhotonNetwork.LocalPlayer.GetWinner())
        {
            trophy.SetActive(true);
            anim.SetBool("Win", true);
            vfx.Play();
            resultText.text = "Win";
        }
        else
            resultText.text = "Lose";
    }

    public void SceneChange()
    {
        PhotonNetwork.Disconnect();
    }
}
