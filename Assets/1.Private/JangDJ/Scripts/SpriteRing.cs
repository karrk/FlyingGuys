using System;
using System.Collections;
using UnityEngine;

public class SpriteRing : MonoBehaviour, IPooledObject
{
    public Enum MyType => E_Sprite.Ring;

    public GameObject MyObject => gameObject;

    private Coroutine _mover;

    private bool _isMoving = false;

    public void Return()
    {
        StopAllCoroutines();
        ObjPoolManager.Instance.ReturnObj(this);
    }

    private const float DIST = 100;
    private static Vector3 _initPos = Vector3.back * DIST;
    private static Vector3 _targetPos = Vector3.zero;

    [SerializeField] private float _timer;

    [SerializeField] public float ArriveDuration;

    public void Move()
    {
        if (_isMoving == true)
            return;

        _isMoving = true;

        transform.position = _initPos;

        _mover = StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        _timer = 0;
        float t = 0;

        while (true)
        {
            if (_timer >= ArriveDuration)
                break;

            _timer += Time.deltaTime;
            t = _timer / ArriveDuration;

            transform.position = Vector3.Lerp(_initPos, _targetPos, t);

            yield return null;
        }

        Return();
    }

    private void OnDisable()
    {
        _isMoving = false;

        if (_mover != null)
            Return();
    }
}
