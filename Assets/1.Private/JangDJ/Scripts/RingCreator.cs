using System.Collections;
using UnityEngine;

public class RingCreator : MonoBehaviour
{
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;

    [SerializeField] private float _minDuration;
    [SerializeField] private float _maxDuration;

    private void OnEnable()
    {
        StartCoroutine(SpawnRings());
    }

    private IEnumerator SpawnRings()
    {
        float timer = 0;
        float duration;
        SpriteRing ring;

        while (true)
        {
            if(timer <= 0)
            {
                ring = ObjPoolManager.Instance.GetObject<SpriteRing>(E_Sprite.Ring);
                
                timer = Random.Range(_minWaitTime, _maxWaitTime);
                duration = Random.Range(_minDuration, _maxDuration);

                ring.ArriveDuration = duration;
                ring.Move();
            }

            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
