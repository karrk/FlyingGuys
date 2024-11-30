using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonShooter : MonoBehaviour
{
    [SerializeField] private float _fireDelay = 1f;
    [SerializeField] private float _arriveTime = 3f;

    [SerializeField] private List<Transform> _muzzlePoints;
    [SerializeField] private List<Transform> _arrivePoints;
    [SerializeField] private bool _switch;

    private Vector3 _muzzlePos;
    private Vector3 _arrivePos;

    private float _gravity;
    private Vector3 _distValue;

    private Coroutine _fireRoutine;

    private void Start()
    {
        _gravity = Physics.gravity.y * -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_switch == true || PhotonNetwork.IsMasterClient == false)
            return;

        else if(other.CompareTag("Player"))
        {
            _switch = true;
            _fireRoutine = StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        WaitForSeconds delay = new WaitForSeconds(_fireDelay);

        while (true)
        {
            SelectMuzzlePos();
            SelectArrivePos();

            _distValue = _arrivePos - _muzzlePos;

            // 수평 속도
            Vector3 horDist = new Vector3(_distValue.x, 0, _distValue.z);
            float horSpeed = horDist.magnitude / _arriveTime;
            Vector3 horVelocity = horDist.normalized * horSpeed;

            // 수직 속도
            float verVelocity = (_distValue.y / _arriveTime) + (0.5f * _gravity * _arriveTime);

            Vector3 resultVelocity = horVelocity + Vector3.up * verVelocity;

            GameObject ball;
            E_RoomObject ballType = SelectBallType();

            int viewID = ObjPoolManager.Instance.GetObjectID(ballType);

            if (viewID == int.MinValue)
                ball = PhotonNetwork.InstantiateRoomObject
                    (ballType == E_RoomObject.Canon ? "CanonBall" : "CanonSmallBall", Vector3.zero, Quaternion.identity);
            else
                ball = PhotonView.Find(viewID).gameObject;

            ball.GetComponent<CanonBall>().Init();
            Rigidbody rb = ball.GetComponent<Rigidbody>();

            rb.transform.position = _muzzlePos;

            rb.angularVelocity = new Vector3(Random.Range(10, 40), 0, Random.Range(-20, 20));
            rb.velocity = resultVelocity;

            yield return delay;
        }
    }

    private E_RoomObject SelectBallType()
    {
        return (E_RoomObject)Random.Range((int)E_RoomObject.Canon_Small, (int)E_RoomObject.Canon + 1);
    }

    private void SelectMuzzlePos()
    {
        _muzzlePos = _muzzlePoints[Random.Range(0, _muzzlePoints.Count)].position;
    }

    private void SelectArrivePos()
    {
        _arrivePos = _arrivePoints[Random.Range(0, _arrivePoints.Count)].position;
    }

    private void OnDisable()
    {
        if (_fireRoutine != null)
            StopCoroutine(_fireRoutine);
    }

}
