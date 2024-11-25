using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class Prefabs<T> where T : Enum
{
    public Type Type
    {
        get
        {
            Debug.Log(typeof(T));
            return typeof(T);
        }
    }

    [SerializeField] private PrefabList<T> _list;

    public GameObject this[T idx]
    {
        get { return _list[idx]; }
    }

    public void CopyToTable()
    {
        _list.RegistAllPrefabs();
    }

    public Dictionary<Enum, GameObject> GetTable()
    {
        var enumTable = new Dictionary<Enum, GameObject>();
        foreach (var kvp in _list.Table)
        {
            enumTable.Add(kvp.Key, kvp.Value);
        }
        return enumTable;
    }
}

[System.Serializable]
public class PrefabList<Detail> where Detail : Enum
{
    public List<PrefabItem<Detail>> List;
    private Dictionary<Detail, GameObject> _table = new Dictionary<Detail, GameObject>();
    public Dictionary<Detail, GameObject> Table => _table;

    public GameObject this[Detail type]
    {
        get { return _table[type]; }
    }

    public void RegistAllPrefabs()
    {
        Detail type;
        GameObject prefab;

        for (int i = 0; i < List.Count; i++)
        {
            type = List[i].Type;
            prefab = List[i].Prefab;

            if (_table.ContainsKey(type) == true)
            { Debug.LogError($"!!!{type} 프리팹 중복등록!!!"); }

            _table.Add(type, prefab);
        }
    }
}

[System.Serializable]
public class PrefabItem<Detail> where Detail : Enum
{
    public Detail Type;
    public GameObject Prefab;
}