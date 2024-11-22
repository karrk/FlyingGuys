using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_StaticBounce : MonoBehaviour
{
    [SerializeField] private Vector3 _bouceDir;
    private Vector3 _tempVec;
    private Vector3 _convertedVec;
    [SerializeField] private float _bounceForce = 10f; // 튕기는 힘 크기
    [SerializeField] private bool _keepVelocityMode;

    private void Start()
    {
        if (_bouceDir == Vector3.zero)
            _bouceDir = transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player") == true)
        {
            if(collision.collider.TryGetComponent<PlayerController>(out PlayerController player))
            {
                if(_keepVelocityMode == true)
                {
                    _tempVec = player.Velocity.normalized;
                    _tempVec.y = 0;
                    _convertedVec = _tempVec + _bouceDir;
                }

                collision.collider.GetComponent<PlayerController>().
                Bounce(_keepVelocityMode == true ? _convertedVec : _bouceDir, _bounceForce);
            }
            
        }
    }
}
