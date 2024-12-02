using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnim : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private float _motionDuration = 5f;

    [SerializeField] private int _clipCount;
    [SerializeField] private float _idleTime;
    private bool _isForceChanged = false;

    private int _lastIdx = 0;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        StartCoroutine(Play());
    }

    public int GetRandomIdx()
    {
        int num = Random.Range(1, _clipCount+1);

        if (num == _lastIdx)
            num = (num + 1) % _clipCount;

        _lastIdx = num;

        return num;
    }

    private IEnumerator Play()
    {
        float timer = 0;
        float interval = 0.1f;
        int motionIdx;
        WaitForSeconds intervalSec = new WaitForSeconds(interval);
        bool isPlaying = false;

        while (true)
        {
            yield return IdlePlay(interval,intervalSec);

            if(_isForceChanged == true)
            {
                _isForceChanged = false;
            }

            while (true)
            {
                if(_isForceChanged == true)
                {
                    break;
                }
                else if(timer >= _motionDuration)
                {
                    break;
                }

                if(isPlaying == false)
                {
                    motionIdx = GetRandomIdx();
                    _anim.SetInteger("Motion", motionIdx);
                    yield return new WaitForEndOfFrame();
                    _anim.SetInteger("Motion", motionIdx + 100);

                    isPlaying = true;
                }
                
                timer += interval;
                yield return intervalSec;
            }

            isPlaying = false;
            timer = 0;
        }
    }

    private IEnumerator IdlePlay(float interval, WaitForSeconds sec)
    {
        float timer = 0;
        bool isPlaying = false;

        while (true)
        {
            if (_isForceChanged == true)
                break;

            else if (timer >= _idleTime)
                break;

            if(isPlaying == false)
            {
                _anim.SetInteger("Motion", 0);
                yield return new WaitForEndOfFrame();
                _anim.SetInteger("Motion", 100);

                isPlaying = true;
            }

            timer += interval;
            yield return sec;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _isForceChanged = true;
    }

}
