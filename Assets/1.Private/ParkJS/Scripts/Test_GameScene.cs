using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class Test_GameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] bool inGamePlay;
    [SerializeField] Char_Spawner charSpawner;
    [SerializeField] TMP_Text countText;
    [SerializeField] OBJ_Crown crown;

    private void Awake()
    {
        crown = GameObject.FindGameObjectWithTag("Target").GetComponent<OBJ_Crown>();
    }

    private void Start()
    {
        if (inGamePlay)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        countText.text = null;
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
        StartCoroutine(ClearRoutine());

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
                StartCoroutine(CountDownRoutine());
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

    IEnumerator CountDownRoutine()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 3; i > 0; i--)
        {
            countText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countText.text = "Go!";
        //NetWorkManager.IsPlay = true;
        yield return new WaitForSeconds(1f);
        countText.gameObject.SetActive(false);
    }

    // TODO : 마스터 클라이언트가 변경되었을 때 현재 존재하는 플레이어의 권한 받기

    IEnumerator ClearRoutine()
    {
        while (true)
        {
            foreach (var item in PhotonNetwork.CurrentRoom.Players.Keys)
            {
                //Debug.Log(item == (crown.Num - 1));
                //Debug.Log($"{item} / {crown.Num}");

                if (NetWorkManager.IsTriggerCrown )//&& item == (crown.Num - 1))
                {
                    //Debug.Log($"{PhotonNetwork.CurrentRoom.Players[crown.Num - 1]}. ");

                    if (PhotonNetwork.CurrentRoom.Players[/*crown.Num-*/1] == PhotonNetwork.LocalPlayer)
                    {
                        // TODO : 승리 연출
                        Debug.Log("승리");
                    }
                    else
                    {
                        // TODO : 패배 연출
                        Debug.Log("패배");
                    }
                    yield break;
                }
            }
            yield return null;
        }
    }
}

