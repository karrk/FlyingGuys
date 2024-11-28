using Photon.Pun;
using UnityEngine;

public class UI_LoadLoadingScene : MonoBehaviour
{
    public void LoadingScene()
    {
        PhotonNetwork.LoadLevel("Public_Loading");
    }
}
