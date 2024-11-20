using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Spawner : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _characterCollider;
    [SerializeField] private int _testCount = 5;
    [SerializeField] private Tr_Ranger _ranger = new Tr_Ranger(); 

    private void Start()
    {
        _ranger.CalculateSize();
        _ranger.SettingPlayerInfo(_characterCollider, 3);
        _ranger.DrawCrossRay();
    }

    public void Spawn(int playerNumber)
    {
        // TODO 플레이어 넘버 = 0부터

        //Vector3 forward = Camera.main.transform.forward;
        //PhotonNetwork.InstantiateRoomObject("Character", _ranger[playerNumber], Quaternion.Euler(forward));
    }
}

[System.Serializable]
public class Tr_Ranger
{
    public float ObjectRadius { get; private set; }
    public int ObjectCount { get; private set; }
    public float Width => _width;
    public float Height => _height;
    public float MinRadius { get; set; }
    
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    private float _width, _height;

    private float _xInterval = float.MinValue;
    private Vector3[] _poses;

    public Vector3 this[int idx]
    {
        get { return _poses[idx]; }
        private set { _poses[idx] = value; }
    }

    public void CalculateSize()
    {
        _width = Mathf.Abs(_start.position.x - _end.position.x);
        _height = Mathf.Abs(_start.position.z - _end.position.z);

        //Debug.Log($"{_width} {_height}");
    }

    public void SettingPlayerInfo(CapsuleCollider coll, int maxPlayerCount)
    {
        this.ObjectRadius = coll.radius;
        this.ObjectCount = maxPlayerCount;
        _poses = new Vector3[maxPlayerCount];

        _xInterval = _width/maxPlayerCount;

        Debug.Log(_xInterval);

        if(CheckUseableRange(ObjectRadius, maxPlayerCount) == false)
        {
            Debug.LogError("스폰 설정범위 초과");
        }
    }

    private bool CheckUseableRange(float radius, int count)
    {
        if (radius * 2 * count > _width)
            return false;

        else if (radius * 2 > _height)
            return false;

        return true;
    }

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
