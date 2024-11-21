using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_GameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] Char_Spawner charSpawner;
    [SerializeField] bool inGamePlay;

    private void Start()
    {
        if (inGamePlay)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        GameStart();
    }

    private void GameStart()
    {
        Debug.Log("게임 시작");

        // TODO : 모든 클라이언트가 실행 하는 곳
        PhotonNetwork.Instantiate("RemoteInput", Vector3.zero, Quaternion.identity);
        photonView.RPC(nameof(PlayerSpawn), RpcTarget.MasterClient);
        PhotonNetwork.LocalPlayer.SetLoad(true);

        if (PhotonNetwork.IsMasterClient == false)
            return;

        // TODO : 마스터 클라이언트만 실행 하는 곳


    }

    [PunRPC]
    private void PlayerSpawn(PhotonMessageInfo info)
    {
        charSpawner.SpawnCharacter(info);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.LOAD))
        {
            Debug.Log($"{targetPlayer.GetPlayerNumber()} 번 플레이 로딩 완료");
            bool allLoad = CheckAllLoad();
            Debug.Log($"모든 플레이어 준비 : {allLoad}");
            if (allLoad)
            {
                Debug.Log("모든 플레이어 준비 완료");
            }
        }
    }

    private bool CheckAllLoad()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false || PhotonNetwork.PlayerList.Length != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                Debug.Log($"1. {player.GetLoad() == false}");
                Debug.Log($"2. {PhotonNetwork.PlayerList.Length != PhotonNetwork.CurrentRoom.MaxPlayers}");
                return false;
            }
        }

        return true;
    }
}

