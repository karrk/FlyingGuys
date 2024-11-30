using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class CanonBall : MonoBehaviourPun, IPooledObject
{
    [SerializeField] private E_RoomObject _type;
    [SerializeField] private float _lifeTime;
    private WaitForSeconds _sec;

    public Enum MyType => _type;

    public GameObject MyObject => gameObject;

    private Coroutine _co;
    private bool _isRequested = false;

    public void Return()
    {
        if (_isRequested == false)
            return;

        ObjPoolManager.Instance.ReturnObj(_type,photonView.ViewID);
    }

    public void Init()
    {
        _isRequested = true;
        _co = StartCoroutine(Wait());
    }

    private void OnDisable()
    {
        if (_co != null)
            StopCoroutine(_co);

        _isRequested = false;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_lifeTime);
        Return();
    }
}
