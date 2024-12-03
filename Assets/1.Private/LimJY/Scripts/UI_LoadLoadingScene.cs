using Photon.Pun;
using UnityEngine;

public class UI_LoadLoadingScene : MonoBehaviour
{
    [SerializeField] bool isGame;

    private void Start()
    {
        if (isGame)
        {
            PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(100, 1000)}";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

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
