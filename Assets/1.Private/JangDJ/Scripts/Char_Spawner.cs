using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Spawner : MonoBehaviour
{
    public static Char_Spawner Instance { get; private set; }

    //[SerializeField] private int _testCount = 5;
    [SerializeField] private Tr_Ranger _ranger = new Tr_Ranger();

    // 생성된 플레이어 게임 오브젝트 목록 // Player 컴포넌트를 받아오는게 좋을것같음
    public List<GameObject> SpawnedPlayers { get; private set; }

    private int _spawnNumber = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnedPlayers = new List<GameObject>();
        _ranger.CalculateSize();
        _ranger.SettingPlayerInfo(3);
        _ranger.DrawCrossRay();
    }

    /// <summary>
    /// 플레이어의 ID를 기반으로 캐릭터를 스폰합니다.
    /// </summary>
    public void SpawnCharacter(PhotonMessageInfo info)
    {
        GameObject newObj = PhotonNetwork.InstantiateRoomObject
            ("Player", _ranger[_spawnNumber++],
            Quaternion.Euler(Camera.main.transform.forward), data: new object[] { info.Sender });

        SpawnedPlayers.Add(newObj);
    }

    private void OnDisable()
    {
        Instance = null;
    }
}

[System.Serializable]
public class Tr_Ranger
{
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    [SerializeField] private float _characterRadius;
    private Vector3[] _poses;

    public float ObjectRadius { get; private set; }
    public int ObjectCount { get; private set; }
    public float MinRadius { get; set; }
    public float Width => _width;
    public float Height => _height;
    
    private float _width, _height;
    private float _xInterval = float.MinValue;
    
    /// <summary>
    /// n 번째 스폰 좌표를 반환합니다.
    /// </summary>
    public Vector3 this[int idx]
    {
        get { return _poses[idx]; }
        private set { _poses[idx] = value; }
    }

    /// <summary>
    /// Start , End의 끝지점을 기반으로 영역의 크기를 설정합니다.
    /// </summary>
    public void CalculateSize()
    {
        _width = Mathf.Abs(_start.position.x - _end.position.x);
        _height = Mathf.Abs(_start.position.z - _end.position.z);

        //Debug.Log($"{_width} {_height}");
    }

    /// <summary>
    /// 캐릭터의 사이즈와 플레이어 수를 확인해 생성 간격을 설정합니다.
    /// </summary>
    public void SettingPlayerInfo(int stagePlayerCount)
    {
        this.ObjectRadius = _characterRadius;
        this.ObjectCount = stagePlayerCount;
        _poses = new Vector3[stagePlayerCount];

        _xInterval = _width/ stagePlayerCount;

        //Debug.Log(_xInterval);

        if(CheckUseableRange(ObjectRadius, stagePlayerCount) == false)
        { Debug.LogError("스폰 설정범위 초과"); }
    }

    private bool CheckUseableRange(float radius, int count)
    {
        if (radius * 2 * count > _width)
            return false;

        else if (radius * 2 > _height)
            return false;

        return true;
    }

    /// <summary>
    /// 디버그용 스폰 각지점의 위치를 표시합니다.
    /// </summary>
    public void DrawCrossRay()
    {
        Vector3 anchor = _start.position - (Vector3.right * _xInterval / 2)
            + Vector3.forward * (_start.position.z > _end.position.z ? -_height/2 : _height/2);

        for (int i = 0; i < ObjectCount; i++)
        {
            anchor.x += _xInterval;
            _poses[i] = anchor;

            Debug.DrawRay(anchor, Vector3.forward * ObjectRadius, Color.red, 10f);
            Debug.DrawRay(anchor, Vector3.back * ObjectRadius, Color.red, 10f);
            Debug.DrawRay(anchor, Vector3.right * ObjectRadius, Color.blue, 10f);
            Debug.DrawRay(anchor, Vector3.left * ObjectRadius, Color.blue, 10f);
        }
    }
}
