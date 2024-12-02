using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test_GameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] bool inGamePlay;
    [SerializeField] Char_Spawner charSpawner;
    [SerializeField] TMP_Text countText;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] OBJ_Crown crown;
    [SerializeField] DeadZone deadZone;

    private bool load;
    private int count;
    private Image winImage;
    private Image loseImage;

    private HashSet<int> _idSet = new HashSet<int>();

    // 이벤트 등록
    public override void OnEnable()
    {
        base.OnEnable();

        if (PhotonNetwork.InRoom)
        {
            PlayerNumbering_OnPlayerNumberingChanged();
        }

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // 이벤트 해제
    public override void OnDisable()
    {
        base.OnDisable();

        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    // 연동 메서드
    private void PlayerNumbering_OnPlayerNumberingChanged()
    {
        foreach (var item in PhotonNetwork.CurrentRoom.Players)
        {
            int id = item.Value.GetPlayerNumber();

            if (id == -1 || _idSet.Contains(id) == true)
                continue;

            _idSet.Add(id);
        }
    }

    // 플레이어 죽었을때 데드존 연동
    public void DeadPlayer(int id)
    {
        _idSet.Remove(id);
        int myId = PhotonNetwork.LocalPlayer.GetPlayerNumber();

        if (myId == id)
            WinOrLose(false);

        if (_idSet.Count <= 1)
        {
            if (_idSet.Contains(myId))
            {
                WinOrLose(true);
                PhotonNetwork.LocalPlayer.SetWinner(true);
            }

            if (PhotonNetwork.IsMasterClient == true)
            {
                StartCoroutine(GoResultScene());
            }

            // 요놈의 프로퍼티에 win ??? 등록시키고
            // 마스터한테 씬넘겨달라 요청하고 

            // 마스터가 씬전환시키고 결과가 나올까 입니다.
        }
    }

    IEnumerator GoResultScene()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel("Public_Result");
    }

    private void Awake()
    {
        if (PhotonNetwork.InRoom)
            inGamePlay = false;

        crown = GameObject.FindGameObjectWithTag("Target")?.GetComponent<OBJ_Crown>();
        deadZone = GameObject.FindGameObjectWithTag("Target")?.GetComponent<DeadZone>();

        winImage = winUI.GetComponentInChildren<Image>();
        loseImage = winUI.GetComponentInChildren<Image>();
        winUI.SetActive(false);
        loseUI.SetActive(false);

        NetWorkManager.IsPlay = false;
    }

    private void Start()
    {
        countText.text = null;
        if (inGamePlay) // 내일 주석 예정
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            //PhotonNetwork.LocalPlayer.NickName = BackendManager.Auth.CurrentUser.DisplayName ?? $"Player {Random.Range(100, 1000)}";
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
        PhotonNetwork.LocalPlayer.SetWinner(false);
        PhotonNetwork.LocalPlayer.SetLoad(true);
        PhotonNetwork.LocalPlayer.SetLife(true);
        StartCoroutine(ClearRoutine());

        if (PhotonNetwork.IsMasterClient == false)
            return;

        // 마스터 클라이언트만 실행 하는 곳
    }

    [PunRPC]
    private void PlayerSpawn(PhotonMessageInfo info)
    {
        charSpawner.SpawnCharacter(info);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.LOAD) && load == false)
        {
            bool allLoad = CheckAllLoad();
            Debug.Log($"모든 플레이어 준비 : {allLoad}");
            if (allLoad && PhotonNetwork.IsMasterClient)
            {
                load = true;
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

        RPCDelegate.Instance.PlayStartFX();
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
                    WinOrLose(true);
                }
                else
                {
                    // TODO : 패배 연출
                    Debug.Log("패배");
                    WinOrLose(false);
                }

                yield return new WaitForSeconds(1f);
                PhotonNetwork.LoadLevel("Public_Result");  // 결과 씬으로 이동
                yield break;
            }

            yield return null;
        }
        #endregion

        while (deadZone != null)
        {
            if (PhotonNetwork.CurrentRoom.Players.Count != PhotonNetwork.CurrentRoom.MaxPlayers || !PhotonNetwork.IsConnected)
            {
                yield return null;
                continue;
            }

            yield return null;
        }
    }

    private void WinOrLose(bool result)
    {
        if(result)
        {
            // Win
            winUI.SetActive(true);
            ShowImage(winImage);
        }
        else
        {
            // Lose
            loseUI.SetActive(true);
            ShowImage(loseImage);
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
}