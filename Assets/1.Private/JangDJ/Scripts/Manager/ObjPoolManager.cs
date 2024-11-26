using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjPoolManager : MonoBehaviour, IManager
{
    public const int InitPoolCount = 5;
    public static ObjPoolManager Instance { get; private set; }

    public Transform MainDirectory { get; private set; }

    private Dictionary<E_PoolType, ObjectPool> _pools
        = new Dictionary<E_PoolType, ObjectPool>();

    #region 등록 프리팹 목록 인스펙터를 통해 확인 가능
    [SerializeField] private Prefabs<E_VFX> vfxPrefabs;
    [SerializeField] private Prefabs<E_Object> objectPrefabs;
    //[SerializeField] private Prefabs<E_MyType> testPrefabs;

    #endregion

    public void Init()
    {
        Instance = this;
        InitDirectory();
        InitPrefabList();
        InitPools();
    }

    private void InitDirectory()
    {
        GameObject obj = new GameObject();
        obj.transform.SetParent(this.transform);
        MainDirectory = obj.transform;
        obj.name = "Pool";
    }

    private void InitPrefabList()
    {
        vfxPrefabs.CopyToTable();
        objectPrefabs.CopyToTable();
    }

    private void InitPools()
    {
        _pools.Add(E_PoolType.VFX, new ObjectPool(typeof(E_VFX),vfxPrefabs.GetTable()));
        _pools.Add(E_PoolType.Object, new ObjectPool(typeof(E_Object), objectPrefabs.GetTable()));
    }

    /// <summary>
    /// Enum 타입을 통해 원하는 게임오브젝트를 반환합니다.
    /// </summary>
    public GameObject GetObject(Enum type)
    {
        Type requestType = type.GetType();

        foreach (var item in _pools)
        {
            var pool = item.Value;
            
            if(pool.Type.Equals(requestType))
            {
                return pool.GetObject(type);
            }
        }

        throw new Exception("등록된 타입, 오브젝트가 아닙니다.");
    }

    /// <summary>
    /// 반환 받을 오브젝트의 컴포넌트를 바로 접근합니다.
    /// 해당 오브젝트의 활성화 로직은 동일하게 동작합니다.
    /// </summary>
    public T GetObject<T>(Enum type)
    {
        return GetObject(type).GetComponent<T>();
    }

    /// <summary>
    /// 대상 오브젝트를 다시 풀로 반환합니다.
    /// </summary>
    public void ReturnObj(IPooledObject obj)
    {
        Type requestType = obj.MyType.GetType();

        foreach (var item in _pools)
        {
            var pool = item.Value;

            if (pool.Type.Equals(requestType))
            {
                pool.Return(obj);
            }
        }
    }

    #region 신규 오브젝트 등록방법
    /*
     * E_Pool 스크립트에서 구분가능한 enum 타입을 정의합니다.
     * 
     * 이펙트 타입이 아닌 아예 다른 타입의 오브젝트라면,
     * OBJ풀 매니저에서 등록 프리팹 목록을 추가해줘야
     * 오브젝트 풀 매니저 인스펙터에 새로운 프리팹 등록 페이지가 추가됩니다.
     * 
     * 오브젝트 풀 매니저 인스펙터 프리팹에서 신규 타입과, 프리팹 오브젝트를 등록합니다.
     * 
     * 풀링 될 오브젝트의 컴포넌트에 IPooledObject 인터페이스를 상속받습니다.
     * 요구하는 구현을 등록합니다. (구체적인 예시는 TestPooledObj 스크립트 참조)
     */
    #endregion

    #region 사용 예시

    //GameObject temp;

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        if (temp == null)
    //            temp = GetObject(E_VFX.Grab);
    //        else
    //        {
    //            temp.GetComponent<IPooledObject>().Return();
    //            temp = null;
    //        }
    //    }
    //}

    #endregion
}