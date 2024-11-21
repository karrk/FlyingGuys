using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_StaticBounce : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private LayerMask _player;

    [SerializeField] private float _bouncePower;

    [SerializeField] private ForceMode _forceMode;

    [SerializeField] private bool _isBounced;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isBounced == true)
            return;

        _isBounced = true;

        if (collision.collider.CompareTag("Player"))
        {
            if(collision.collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Vector3 normal = collision.collider.ClosestPoint(rb.transform.position);

                Vector3 dir = normal.normalized;

                rb.velocity = Vector3.zero;
                rb.AddForce(_bouncePower * dir, _forceMode);

                StartCoroutine(WaitBounceDealy());
            }
        }
    }

    private void BounceObject(Rigidbody rb)
    {
        Vector3 dir = (rb.position - _pivot.position).normalized;

        rb.velocity = Vector3.zero;
        rb.AddForce(_bouncePower * dir, _forceMode);
    }

    private IEnumerator WaitBounceDealy()
    {
        yield return new WaitForSeconds(0.2f);

        _isBounced = false;
    }
}
