using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Rigidbody))]
public class OBJ_Hurdle_Roller : MonoBehaviour
{
    protected Rigidbody _rb;
    [SerializeField] public float Speed;
    [SerializeField] protected Axis _axis;
    [SerializeField] private Transform _com;
    private ConfigurableJoint _joint;
    private Vector3 _rotDir;


    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _joint = GetComponent<ConfigurableJoint>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.mass = 10000;
        _rb.centerOfMass = _com.position - transform.position;

        AxisSetup();
    }

    private void AxisSetup()
    {
        _joint.xMotion = ConfigurableJointMotion.Locked;
        _joint.yMotion = ConfigurableJointMotion.Locked;
        _joint.zMotion = ConfigurableJointMotion.Locked;

        switch (_axis)
        {
            case Axis.X:
                _rotDir = transform.right.normalized;
                _joint.angularXMotion = ConfigurableJointMotion.Free;
                _joint.angularYMotion = ConfigurableJointMotion.Locked;
                _joint.angularZMotion = ConfigurableJointMotion.Locked;

                break;
            case Axis.Y:
                _rotDir = transform.up.normalized;
                _joint.angularXMotion = ConfigurableJointMotion.Locked;
                _joint.angularYMotion = ConfigurableJointMotion.Free;
                _joint.angularZMotion = ConfigurableJointMotion.Locked;
                break;
            case Axis.Z:
                _rotDir = transform.forward.normalized;
                _joint.angularXMotion = ConfigurableJointMotion.Locked;
                _joint.angularYMotion = ConfigurableJointMotion.Locked;
                _joint.angularZMotion = ConfigurableJointMotion.Free;
                break;
        }
    }


    protected virtual void FixedUpdate()
    {
        _rb.angularVelocity = _rotDir * Speed;
    }
}