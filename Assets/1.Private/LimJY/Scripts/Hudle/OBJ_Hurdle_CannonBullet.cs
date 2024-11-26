using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_Hurdle_CannonBullet : MonoBehaviour, IPooledObject
{
    public  float returnTime;
    private float remainTime;

    public Enum MyType => E_Object.Cannon;
    public GameObject MyObject => gameObject;



    private void OnEnable()
    {
        remainTime = returnTime;
    }

    private void OnDisable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void Update()
    {
        remainTime -= Time.deltaTime;

        if (remainTime < 0)
        {
            Return();
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(this);
    }
}
