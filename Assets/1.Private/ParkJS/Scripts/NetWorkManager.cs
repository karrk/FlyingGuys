using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkManager : MonoBehaviourPunCallbacks, IManager
{
    public static NetWorkManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버");
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom("Room", options, TypedLobby.Default);
    }


}
