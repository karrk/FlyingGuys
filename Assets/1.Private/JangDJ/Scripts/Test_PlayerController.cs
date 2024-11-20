using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerController : MonoBehaviour
{
    [SerializeField] private CamController _cam;

    [SerializeField] private float _speed = 1f;
    
    [SerializeField] private Rigidbody _rb;

    private Vector3 _moveVec;
    private Vector3 _rotVec;

    private Vector3 _dir;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        _moveVec.x = Input.GetAxisRaw("Horizontal");
        _moveVec.z = Input.GetAxisRaw("Vertical");

        _dir = cameraForward * _moveVec.z + cameraRight * _moveVec.x;

        Move(_dir.normalized);

        // 마우스 X값은 사용 안합니다.
        Rotate(Input.GetAxisRaw("Mouse Y"));
    }

    private void Move(Vector3 dir)
    {
        _rb.velocity = _speed * dir + Vector3.up * _rb.velocity.y;
    }

    private void Rotate(float value)
    {
        _cam.RotY(value);
    }
}
