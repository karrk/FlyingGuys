using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OBJ_Hurdle_Rotation : MonoBehaviour
{
    protected Rigidbody _rb;
    [SerializeField] protected Vector3 _rotateSpeed;
    [SerializeField] private Transform _com;


    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _com.position - transform.position;
    }

    protected virtual void FixedUpdate()
    {
        Quaternion Rotate = Quaternion.Euler(_rotateSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * Rotate);
    }
}
