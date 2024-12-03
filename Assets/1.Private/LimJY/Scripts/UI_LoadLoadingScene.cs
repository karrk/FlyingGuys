using Photon.Pun;
using UnityEngine;

public class UI_LoadLoadingScene : MonoBehaviour
{
    // MainMenu -> Loading
    public void LoadingScene()
    {
        PhotonNetwork.LoadLevel("Public_Loading");
    }

    // Result -> Loading
    public void RestartScene()
    {
        NetWorkManager.Restarter = true;
        PhotonNetwork.LeaveRoom();
    }
}
