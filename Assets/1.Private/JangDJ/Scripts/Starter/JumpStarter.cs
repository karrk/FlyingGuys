using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStarter : MonoBehaviour, IStarter
{
    private const float CHANGE_DURATION = 1f;
    private const int REMAIN_GROUNDS = 2;
    private const float FALL_TIME_INTERVAL = 3f;

    [SerializeField] private Animation _anim;
    [SerializeField] private List<JumpSpeed> _speeds;

    [SerializeField] private OBJ_Hurdle_Roller _r1;
    [SerializeField] private OBJ_Hurdle_Roller _r2;

    [SerializeField, Space(20)]
    private List<Rigidbody> _grounds;

    private Coroutine _changer;
    private Coroutine _rotater;

    private int _speedIdx = 0;

    public void StartStage()
    {
        StartCoroutine(JumpStart());
                    
    }

    private IEnumerator JumpStart()
    {
        _anim.Play();
        yield return StageFXs.Instance.StartCountDown();
        NetWorkManager.IsPlay = true;
        StartCoroutine(StageFXs.Instance.StepPlayFX());

        if (PhotonNetwork.IsMasterClient == true)
        {
            _rotater = StartCoroutine(RotateRoutine());
            StartCoroutine(FallGround());
        }
    }

    private IEnumerator FallGround()
    {
        float timer = 0;

        while (true)
        {
            if (_grounds.Count <= 2)
                break;

            else if(timer >= FALL_TIME_INTERVAL)
            {
                Rigidbody ground = _grounds[Random.Range(0, _grounds.Count)];
                StartCoroutine(FallRoutine(ground));
                _grounds.Remove(ground);
                timer = 0;
            }

            timer += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator FallRoutine(Rigidbody target)
    {
        Vector3 initPos = target.position;
        Vector3 tempPos;

        float shakePower = DropFloor.MAX_SHAKE_POWER;
        float shakeTime = 3f;
        float timer = 0;

        while (true)
        {
            if(timer >= shakeTime) { break; }

            tempPos.x = initPos.x - Random.Range(-shakePower, shakePower);
            tempPos.y = initPos.y - Random.Range(-shakePower, shakePower);
            tempPos.z = initPos.z - Random.Range(-shakePower, shakePower);

            target.transform.position = tempPos;

            timer += DropFloor.TIMESEC;
            yield return DropFloor.TimeSec;
        }

        target.isKinematic = false;
    }

    private IEnumerator RotateRoutine()
    {
        float duration = _speeds[_speedIdx].Duration;

        StartCoroutine(ChangeSpeed());

        while (true)
        {
            yield return new WaitForSeconds(duration);

            _speedIdx++;

            if (_speedIdx >= _speeds.Count)
                yield break;

            duration = _speeds[_speedIdx].Duration;

            if (_changer != null)
                StopCoroutine(_changer);

            _changer = StartCoroutine(ChangeSpeed());
        }
    }

    private IEnumerator ChangeSpeed()
    {
        float t = 0;
        float timer = 0;

        float r1Speed = _r1.Speed;
        float r2Speed = _r2.Speed;

        float r1Target = _speeds[_speedIdx].Roller1;
        float r2Target = _speeds[_speedIdx].Roller2;

        while (true)
        {
            if(t >= 1) { break; }

            timer += Time.deltaTime;
            t = timer / CHANGE_DURATION;

            _r1.Speed = Mathf.Lerp(r1Speed, r1Target, t);
            _r2.Speed = Mathf.Lerp(r2Speed, r2Target, t);

            yield return null;
        }

        _r1.Speed = r1Target;
        _r2.Speed = r2Target;
    }

    private void OnDisable()
    {
        if (_changer != null)
            StopCoroutine(_changer);

        if (_rotater != null)
            StopCoroutine(_rotater);
    }

    [System.Serializable]
    private class JumpSpeed
    {
        public float Roller1;
        public float Roller2;
        public float Duration;
    }


}


