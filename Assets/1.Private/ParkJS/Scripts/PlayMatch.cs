using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayMatch : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] float waitTime;
    [SerializeField] public int num { get; private set; }
    public event Action onChangeStageNum;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        SetDescriptionText("Server connecting...");
    }

    [PunRPC]
    private void SetDescriptionText(string msg)
    {
        descriptionText.text = msg;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 진입");
        SetDescriptionText("Wait for Players...");
        PhotonNetwork.LocalPlayer.SetReady(true);

        if (PhotonNetwork.IsMasterClient == false)
            return;

        num = Random.Range(0, 4);
        Debug.Log($"선정된 수 {num}");
        onChangeStageNum();
    }



    Coroutine playGameRoutine;
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        newPlayer.SetWinner(false);
        newPlayer.SetLife(true);

        if (PhotonNetwork.IsMasterClient == false)
            return;

        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            StopCoroutine(playGameRoutine);
            return;
        }

        else if(playGameRoutine != null)
        {
            Debug.Log("실행 중인 코루틴이 있음");
            StopCoroutine(playGameRoutine);
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            playGameRoutine = StartCoroutine(PlayGameRoutine());
        }
    }

    IEnumerator PlayGameRoutine()
    {
        for (int i = (int)waitTime; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log($"{i} s");
        }

        Debug.Log("게임 시작할 예정");
        StartCoroutine(WaitPlayerRoutine());
        yield return null;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.READY))
        {
            bool allReady = CheckAllReady();
            Debug.Log($"모든 플레이어 준비 : {allReady}");
            if (allReady && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                StartCoroutine(WaitPlayerRoutine());
            }
        }
    }

    private bool CheckAllReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady() == false)
            {
                Debug.Log($"{player.GetPlayerNumber()}. {player.GetReady()}");
                return false;
            }
        }

        return true;
    }

    IEnumerator WaitPlayerRoutine()
    {
        photonView.RPC(nameof(SetDescriptionText), RpcTarget.All, "Game Loading...");
        yield return new WaitForSeconds(3f);

        if (PhotonNetwork.IsMasterClient == false)
            yield break;

        ChoiceGameScene();
    }

    private void ChoiceGameScene()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        switch (num)
        {
            case 0: // 산 무너져유
                PhotonNetwork.LoadLevel("TestSecne 1.4");
                break;
            case 1: // 바닥 떨어져유
                PhotonNetwork.LoadLevel("Stage2");
                break;
            case 2: // 점프 쇼다운
                PhotonNetwork.LoadLevel("Stage3");
                break;
            case 3: // 롤아웃
                PhotonNetwork.LoadLevel("Stage4");
                break;
        }
    }
}
