using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectPool
{
    private Dictionary<E_RoomObject, List<int>> _pools = new Dictionary<E_RoomObject, List<int>>();
    private Dictionary<E_RoomObject, Transform> _directorys = new Dictionary<E_RoomObject, Transform>();

    private Transform _directory;

    public RoomObjectPool()
    {
        CreateMainDirectory();

        for (int i = 0; i < (int)E_RoomObject.Size; i++)
        {
            _pools.Add((E_RoomObject)i, new List<int>());

            Transform newDir = new GameObject().transform;
            newDir.parent = _directory;
            newDir.name = ((E_RoomObject)i).ToString();
            _directorys.Add((E_RoomObject)i, newDir);
        }
    }

    public void ClearPools()
    {
        foreach (var e in _pools)
        {
            if (e.Value.Count >= 0)
                e.Value.Clear();
        }
    }

    private void CreateMainDirectory()
    {
        _directory = new GameObject().transform;
        _directory.name = "RoomObjectPool";
        _directory.transform.parent = ObjPoolManager.Instance.MainDirectory;
    }

    public int GetObjectID(E_RoomObject objType)
    {
        if (_pools[objType].Count <= 0)
            return int.MinValue;

        List<int> list = _pools[objType];
        int id = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        RPCDelegate.Instance.SetActive(id, true);

        return id;
    }

    public void ReturnObj(E_RoomObject type, int id)
    {
        _pools[type].Add(id);
        RPCDelegate.Instance.SetActive(id, false);
    }
}
