using System.Collections;
using UnityEngine;

public class DropFloor : MonoBehaviour
{
    private const float TIMESEC = 0.1f;

    private static float MAX_SHAKE_POWER = 0.075f;
    private static float SHAKE_TIME = 1.5f;

    private static WaitForSeconds TimeSec = new WaitForSeconds(TIMESEC);

    private Vector3 _initPos;
    private Vector3 _tempVec;

    private bool _isDestroyRequested = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDestroyRequested == true)
            return;

        if(collision.collider.CompareTag("Player"))
        {
            _isDestroyRequested = true;

            _initPos = transform.position;
            StartCoroutine(ShakeRoutine());
        }
    }

    private IEnumerator ShakeRoutine()
    {
        float timer = 0;

        while (true)
        {
            if(timer >= SHAKE_TIME) { break; }

            Shake();

            timer += TIMESEC;
            yield return TimeSec;
        }

        RPCDelegate.Instance.DestroyDropFloor(_initPos);
    }

    private void Shake()
    {
        _tempVec.x = _initPos.x - Random.Range(-MAX_SHAKE_POWER, MAX_SHAKE_POWER);
        _tempVec.y = _initPos.y - Random.Range(-MAX_SHAKE_POWER, MAX_SHAKE_POWER);
        _tempVec.z = _initPos.z - Random.Range(-MAX_SHAKE_POWER, MAX_SHAKE_POWER);

        transform.position = _tempVec;
    }


}
