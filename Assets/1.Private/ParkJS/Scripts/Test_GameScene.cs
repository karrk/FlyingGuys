using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test_GameScene : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] bool inGamePlay;
    [SerializeField] Char_Spawner charSpawner;
    [SerializeField] TMP_Text countText;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] OBJ_Crown crown;
    [SerializeField] DeadZone deadZone;

    [SerializeField] private bool alone;
    private int count;
    private Image winImage;
    private Image loseImage;

    private void Awake()
    {
        crown = GameObject.FindGameObjectWithTag("Target")?.GetComponent<OBJ_Crown>();
        deadZone = GameObject.FindGameObjectWithTag("Target")?.GetComponent<DeadZone>();

        winImage = winUI.GetComponentInChildren<Image>();
        loseImage = winUI.GetComponentInChildren<Image>();
        winUI.SetActive(false);
        loseUI.SetActive(false);
    }

    private void Start()
    {
        countText.text = null;

        if (inGamePlay)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LocalPlayer.NickName = NetWorkManager.NickName ?? $"Player {Random.Range(100, 1000)}";
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (PhotonNetwork.InRoom)
        {
            Debug.Log("In Game View");
            StartCoroutine(StartDelayRoutine());
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
        // 모든 클라이언트가 실행 하는 곳
        PhotonNetwork.Instantiate("RemoteInput", Vector3.zero, Quaternion.identity);
        photonView.RPC(nameof(PlayerSpawn), RpcTarget.MasterClient);
        PhotonNetwork.LocalPlayer.SetLoad(true);
        PhotonNetwork.LocalPlayer.SetLife(true);
        PhotonNetwork.LocalPlayer.SetWinner(false);
        StartCoroutine(ClearRoutine());

        if (PhotonNetwork.IsMasterClient == false)
            return;

        // 마스터 클라이언트만 실행 하는 곳
    }

    [PunRPC]
    private void PlayerSpawn(PhotonMessageInfo info)
    {
        charSpawner.SpawnCharacter(info);
        //NetWorkManager.Instance.PlayerList.Add(info.Sender);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.LOAD))
        {
            bool allLoad = CheckAllLoad();
            Debug.Log($"모든 플레이어 준비 : {allLoad}");
            if (allLoad && PhotonNetwork.IsMasterClient)
            {
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
                return false;
            }
        }

        return true;
    }

    IEnumerator CountDownRoutine()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 3; i >= 0; i--)
        {
            photonView.RPC(nameof(ShowCount), RpcTarget.All, i);
            yield return new WaitForSeconds(1f);
        }

        //NetWorkManager.IsPlay = true;
        photonView.RPC(nameof(HideText), RpcTarget.All, false);
    }

    [PunRPC]
    private void ShowCount(int count)
    {
        if (count <= 0)
        {
            countText.text = "Go!";
            return;
        }

        countText.text = count.ToString();
    }

    [PunRPC]
    private void HideText(bool result)
    {
        countText.gameObject.SetActive(result);
    }

    IEnumerator ClearRoutine()
    {
        #region MountainDown
        while (crown != null)
        {
            if (NetWorkManager.IsTriggerCrown)
            {
                if (crown.player == PhotonNetwork.LocalPlayer)
                {
                    // TODO : 승리 연출
                    Debug.Log("승리");
                    PhotonNetwork.LocalPlayer.SetWinner(true);
                    winUI.SetActive(true);
                    ShowImage(winImage);
                }
                else
                {
                    // TODO : 패배 연출
                    Debug.Log("패배");
                    loseUI.SetActive(true);
                    ShowImage(loseImage);
                }

                yield return new WaitForSeconds(1f);
                PhotonNetwork.LoadLevel("UI_End");  // 결과 씬으로 이동
                yield break;
            }

            yield return null;
        }
        #endregion

        while (deadZone != null)
        {
            alone = NetWorkManager.IsAlone;

            if (PhotonNetwork.CurrentRoom.Players.Count != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                yield return null;
                continue;
            }

            if (deadZone.player == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("패배");
                Debug.Log(deadZone.player);
            }
            //else if (NetWorkManager.IsAlone)
            //{
            //    Debug.Log("승리");
            //}
            else
            {
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.GetLife())
                    {
                        count++;
                    }
                }

                if (count <= 1)
                    Debug.Log("확인용");
                else
                    count = 0;
            }

            yield return null;
        }
    }

    private void ShowImage(Image iamge)
    {
        iamge.fillAmount = 0;
        while (iamge.fillAmount < 1.0f)
        {
            iamge.fillAmount += 2f * Time.deltaTime;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Util.SendAndReceiveStruct(stream, ref count);
        Debug.Log($"전달되는 값 : {count}");
        Util.SendAndReceiveStruct(stream, ref alone);
        Debug.Log($"전달되는 값 : {alone}");
    }
}