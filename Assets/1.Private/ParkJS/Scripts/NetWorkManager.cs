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

    private bool isPlay;
    public static bool IsPlay { get { return Instance.isPlay; } set { Instance.isPlay = value; } }

    private bool isTriggerCrown;
    public static bool IsTriggerCrown { get { return Instance.isTriggerCrown; } set { Instance.isTriggerCrown = value; } }

    // 백엔드 생성전 이름 저장 공간
    private string nickName;
    public static string NickName { get { return Instance.nickName; } set { Instance.nickName = value; } }

    public void Init()
    {
        Instance = this;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Master Server Connect");
        IsTriggerCrown = false;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom("Room11", options, TypedLobby.Default);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방 나가기");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"연결 종료 : {cause}");
        SceneManager.LoadScene("UI_MainMenu");
    }

}
