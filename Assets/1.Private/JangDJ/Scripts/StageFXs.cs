using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFXs : MonoBehaviour
{
    public static StageFXs Instance;

    [SerializeField] private float _delay;
    [SerializeField] private int[] _stepPoints;
    [SerializeField] private ParticleSystem[] _particles;
    
    private void OnEnable()
    {
        Instance = this;
    }

    public void PlayStartFX()
    {
        StartCoroutine(StepPlayFX());
    }

    private IEnumerator StepPlayFX()
    {
        if(_stepPoints == null || _stepPoints.Length <= 0)
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                _particles[i].Play();
            }
        }

        else
        {
            int tempIdx = 0;
            WaitForSeconds wait = new WaitForSeconds(_delay);

            for (int i = 0; i < _particles.Length; i++)
            {
                _particles[i].Play();

                if (tempIdx <= _stepPoints.Length -1 && _stepPoints[tempIdx] == i)
                {
                    tempIdx++;
                    yield return wait;
                }
                    
            }
        }
    }

    private void OnDisable()
    {
        Instance = null;
    }
}
