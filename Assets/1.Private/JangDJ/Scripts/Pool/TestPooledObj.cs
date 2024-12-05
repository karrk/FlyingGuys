using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPooledObj : MonoBehaviour, IPooledObject
{
    public Enum MyType => E_VFX.Grab;

    public GameObject MyObject => gameObject;

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(this);
    }
}
