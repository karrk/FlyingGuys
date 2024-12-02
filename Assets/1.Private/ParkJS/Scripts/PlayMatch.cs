using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayMatch : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] float waitTime;
    [SerializeField] int num;

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
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Room", new RoomOptions { MaxPlayers = 2, IsVisible = false });
        // 2명이 입장상태가 확인되면, 5초?? 더 기다리고 그냥 시작 => 가능??
    }

    Coroutine playGameRoutine;
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
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
            // 2명이 입장한 상태
            // 지정한 시간 후 참여인원이 없으면 게임 시작
            // 참여하는 인원이 있으면 지정된 시간 초기화
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
        num = Random.Range(0, 4);
        Debug.Log($"당첨된 수 {num}");
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
