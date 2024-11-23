using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPool
{
    public Type Type { get; private set; }
    private Dictionary<Enum, List<GameObject>> _pools = new Dictionary<Enum, List<GameObject>>();
    private Dictionary<Enum, Transform> _directorys = new Dictionary<Enum, Transform>();

    private Transform _typeDirectory;

    public ObjectPool(Type type, IEnumerable<KeyValuePair<Enum, GameObject>> table)
    {
        Type = type;

        _typeDirectory = CreateDirectory(ObjPoolManager.Instance.MainDirectory, _typeDirectory, $"{type.Name} Pool");

        List<GameObject> tempList;

        foreach (var item in table)
        {
            tempList = new List<GameObject>();

            _directorys.Add(item.Key, new GameObject().transform);
            _pools.Add(item.Key, tempList);

            CreateDirectory(_typeDirectory, _directorys[item.Key], $"{item.Value.name} pool");
            CreatePool(tempList, item.Value, _directorys[item.Key]);
        }
    }

    private void CreatePool(List<GameObject> poolList, GameObject prefab, Transform parent)
    {
        GameObject newObj;

        for (int i = 0; i < ObjPoolManager.InitPoolCount; i++)
        {
            newObj = GameObject.Instantiate(prefab);
            newObj.transform.SetParent(parent);
            newObj.SetActive(false);
            poolList.Add(newObj);
        }
    }

    public GameObject GetObject(Enum type)
    {
        List<GameObject> pool = _pools[type];

        GameObject obj = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);

        obj.SetActive(true);
        return obj;
    }

    public void Return(IPooledObject obj)
    {
        List<GameObject> pool = _pools[obj.MyType];
        pool.Add(obj.MyObject);
        obj.MyObject.SetActive(false);
    }

    private Transform CreateDirectory(Transform parent, Transform child, string name)
    {
        if (child == null)
            child = new GameObject().transform;

        child.transform.parent = parent;
        child.name = name;

        return child;
    }
}