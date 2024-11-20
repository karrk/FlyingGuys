using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera playerCam;
    [SerializeField] Transform cameraArm;
    [SerializeField] float moveSpeed;
    public Vector2 moveInput;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        HandleInput();

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void LateUpdate()
    {
        LookAround();
    }

    private void HandleInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;
    }

    private void MovePlayer()
    {
        bool isMove = moveInput.sqrMagnitude != 0;
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            transform.forward = lookForward;
            //playerBody.forward = lookForward;
            rb.velocity = moveDir * moveSpeed;

            //transform.position += moveDir * moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (mouseDelta.sqrMagnitude == 0)
            return;

        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            // 카메라 각도 제한
            // 0으로 잡으면 카메라가 수평면 아래로 내려가지 않음
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

}
