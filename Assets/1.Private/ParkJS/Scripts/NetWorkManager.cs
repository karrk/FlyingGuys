using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetWorkManager : MonoBehaviourPunCallbacks, IManager
{
    public static NetWorkManager Instance { get; private set; }
    public static bool[,] PlayerResults = new bool[8, 2];

    private bool isPlay;
    public static bool IsPlay { get { return Instance.isPlay; } set { Instance.isPlay = value; } }

    private bool isTriggerCrown;
    public static bool IsTriggerCrown { get { return Instance.isTriggerCrown; } set { Instance.isTriggerCrown = value; } }

    public static bool Restarter = false;

    public void Init()
    {
        Instance = this;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Master Server Connect");
        IsTriggerCrown = false;
        
        if (SceneManager.GetActiveScene().name == "Public_Loading")
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방 나가기");
        if (SceneManager.GetActiveScene().name == "Public_Result")
        {
            if(Restarter)
                SceneManager.LoadScene("Public_Loading");
            else
                SceneManager.LoadScene("Public_Menu");

            Restarter = false;
        }
        else
        {
            SceneManager.LoadScene("Public_Menu");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"실패 사유 : {message}");
        PhotonNetwork.CreateRoom($"Room {Random.Range(100, 1000)}", new RoomOptions { MaxPlayers = 8 }, TypedLobby.Default);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom($"Room {Random.Range(100, 1000)}", new RoomOptions { MaxPlayers = 8 }, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"실패 사유 : {message}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"연결 종료 : {cause}");
    }

}
