using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerStarter : StageStarter
{
    private const float CHANGE_DURATION = 3f;

    [SerializeField] private GameObject _emptyFloor;
    [SerializeField] private Animation _anim;
    [SerializeField] private OBJ_Hurdle_Roller _r1;

    [SerializeField] private List<RollerSpeed> _speeds;

    private Coroutine _changer;
    private Coroutine _rotater;

    private int _speedIdx = 0;

    public override void StageStart()
    {
        StartCoroutine(RollerStart());
    }

    private IEnumerator RollerStart()
    {
        _anim.Play();
        yield return StageFXs.Instance.StartCountDown();
        _emptyFloor.SetActive(false);
        NetWorkManager.IsPlay = true;
        StartCoroutine(StageFXs.Instance.StepPlayFX());

        if(PhotonNetwork.IsMasterClient == true)
        {
            _rotater = StartCoroutine(RotateRoutine());
        }
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

        float r1Target = _speeds[_speedIdx].Roller1;

        while (true)
        {
            if (t >= 1) { break; }

            timer += Time.deltaTime;
            t = timer / CHANGE_DURATION;

            _r1.Speed = Mathf.Lerp(r1Speed, r1Target, t);

            yield return null;
        }

        _r1.Speed = r1Target;
    }

    [System.Serializable]
    private class RollerSpeed
    {
        public float Roller1;
        public float Duration;
    }
}
