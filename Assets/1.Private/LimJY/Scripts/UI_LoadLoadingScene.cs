using Photon.Pun;
using UnityEngine;

public class UI_LoadLoadingScene : MonoBehaviour
{
    public void LoadingScene()
    {
        PhotonNetwork.LoadLevel("Public_Loading");
    }

    public void RestartScene()
    {
        NetWorkManager.Restarter = true;
        PhotonNetwork.LeaveRoom();
    }
}
