using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    public Rigidbody rb;
    public Vector3 moveDir;
    public bool isJumpable;
    public bool isDiving;
    [SerializeField] Vector3 rotVec;

    public PlayerModel model;
    public PlayerView view;

    private Player player;
    //[SerializeField] int playerNumber;
    //[SerializeField] float moveSpeed;
    //[SerializeField] float jumpForce;

    [SerializeField] CamController _cam;


    // 버퍼
    public float jumpBufferTime = 0.2f;
    public float jumpBufferCounter;


    // 바닥 탐지
    // 미리 계산한 값을 포인트로 잡아줬음, 캐릭터 스케일이 변한다면 트랜스폼을 계산해 재지정하는 코드를 넣어줘야할것
    [SerializeField] private Transform rayPoint1;
    [SerializeField] private Transform rayPoint2;
    [SerializeField] private Transform rayPoint3;

    private RaycastHit groundhit1;
    private RaycastHit groundhit2;
    private RaycastHit groundhit3;

    public RaycastHit chosenHit;

    private float rayLength = 0.5f;

    public bool isGrounded;
    public bool isSlope;
    public float groundAngleValue;
    public Vector3 perpAngle;


    // 상태
    [SerializeField] E_PlayeState curState;
    private PlayerState[] states = new PlayerState[(int)E_PlayeState.Size];

    // 물리 충돌 레이어
    private int obstacleLayer;

    // 임시 변수
    public float bouncedForce = float.MinValue; // 충돌한 장애물에서 받아오는게 더 적합해보임
    public Vector3 bouncedDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = (Player)photonView.InstantiationData[0];
        model.playerNumber = player.GetPlayerNumber();
        view = GetComponent<PlayerView>();
        rayPoint1 = transform.Find("RayPoints/rayPointForward");
        rayPoint2 = transform.Find("RayPoints/rayPointLeft");
        rayPoint3 = transform.Find("RayPoints/rayPointRight");

        // 상태
        states[(int)E_PlayeState.Idle] = new IdleState(this);
        states[(int)E_PlayeState.Run] = new RunState(this);
        states[(int)E_PlayeState.Jump] = new JumpState(this);
        states[(int)E_PlayeState.Fall] = new FallState(this);
        states[(int)E_PlayeState.Diving] = new DivingState(this);
        states[(int)E_PlayeState.Bounced] = new BouncedState(this);
    }

    private void Start()
    {
        if(player == PhotonNetwork.LocalPlayer)
        {
            _cam.SetForcePriority(10);
        }

        curState = E_PlayeState.Idle;
        states[(int)curState].Enter();

        // 레이어 미리 캐싱
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
    }

    private void Update()
    {
        HandleCamInput();

        if (!photonView.IsMine)
            return;

        HandleMoveInputs();
        ControlJumpBuffer();

        states[(int)curState].Update();

        //Debug.Log(rb.velocity);

        //if (RemoteInput.inputs[model.playerNumber].jumpInput && !isJumping)
        //{
        //    Debug.Log("점프 입력됨");


        //    //JumpTemp();
        //    //RemoteInput.inputs[model.playerNumber].jumpInput = false;
        //}


    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        
        states[(int)curState].FixedUpdate();
        CheckGround();

        if(!isGrounded && curState != E_PlayeState.Bounced)
        {
            MoveInAir();
        }

        //moveDir = RemoteInput.inputs[playerNumber].MoveDir;
        //rotVec = RemoteInput.inputs[playerNumber].RotVec;
        //rb.velocity = moveDir.normalized * model.moveSpeed + Vector3.up * rb.velocity.y;
    }

    private void LateUpdate()
    {
        _cam.RotY(rotVec.y);
        //states[(int)curState].LateUpdate();
    }


    public void ChangeState(E_PlayeState newState)
    {
        states[(int)curState].Exit();
        curState = newState;
        states[(int)curState].Enter();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == obstacleLayer)
        {
            if(collision.gameObject.TryGetComponent<BounceObject>(out BounceObject bounceObject))
            {
                bouncedForce = bounceObject.Power;
                bouncedDir = collision.contacts[0].normal.normalized;
                ChangeState(E_PlayeState.Bounced);
            }
        }
    }

    public void MoveInAir()
    {
        // Run의 로직을 속도만 다르게한것 아마도 수정 필요

        if (moveDir == Vector3.zero)
            return;

        Vector3 targetVelocity = moveDir * model.moveSpeedInAir;
        targetVelocity.y = rb.velocity.y;

        rb.velocity = targetVelocity;

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.fixedDeltaTime * 15 // 수정필요
        );



    }

    //private void JumpTemp()
    //{
    //    if (RemoteInput.inputs[model.playerNumber].jumpInput)
    //    {
    //        Debug.Log("점프 입력됨");
    //        rb.AddForce(Vector3.up * model.jumpForce, ForceMode.Impulse);
    //        RemoteInput.inputs[model.playerNumber].jumpInput = false;
    //    }

    //}

    public void HandleMoveInputs()
    {
        moveDir = RemoteInput.inputs[model.playerNumber].MoveDir;
    }

    private void ControlJumpBuffer()
    {
        //점프 인풋이 들어왔으면
        if (RemoteInput.inputs[model.playerNumber].jumpInput)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
            jumpBufferCounter -= Time.deltaTime;
    }

    private void HandleCamInput()
    {
        //Debug.Log(_cam.gameObject.name);
        rotVec = RemoteInput.inputs[model.playerNumber].RotVec;
    }

    private void CheckGround()
    {
        //if (rb.velocity.y > 0)
        //{
        //    return isGrounded = false;
        //}

        bool rayhit1 = Physics.Raycast(rayPoint1.position, Vector3.down, out groundhit1, rayLength);
        bool rayhit2 = Physics.Raycast(rayPoint2.position, Vector3.down, out groundhit2, rayLength);
        bool rayhit3 = Physics.Raycast(rayPoint3.position, Vector3.down, out groundhit3, rayLength);

        isGrounded = rayhit1 || rayhit2 || rayhit3;

        if(isGrounded)
        {
            // isGrounded가 true라면 셋중 어느 하나는 히트됐다는 뜻
            chosenHit = rayhit1 ? groundhit1 : (rayhit2 ? groundhit2 : groundhit3);

            // chosenHit 선정, distance가 더 짧은 hit를 선정
            if(rayhit1 && rayhit2)
                chosenHit = groundhit1.distance <= groundhit2.distance ? groundhit1 : groundhit2;
            if (rayhit3 && chosenHit.distance > groundhit3.distance)
            {
                chosenHit = groundhit3;
            }

            // 각도 계산
            perpAngle = Vector3.Cross(chosenHit.normal,Vector3.up).normalized; //3D는 Perpendicular가 아닌 cross를 써야함, 외적을 계산해 수직벡터를 구하는 방식
            groundAngleValue = Vector3.Angle(chosenHit.normal, Vector3.up);

            isSlope = groundAngleValue > 0;

            Debug.DrawLine(chosenHit.point, chosenHit.point + chosenHit.normal, Color.blue); // 법선 벡터
            Debug.DrawLine(chosenHit.point, chosenHit.point + perpAngle, Color.red);        // 법선 벡터와 수직인 벡터
        }



        //isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.12f, Vector3.down, out RaycastHit hitInfo, rayLength);
        //if (hitInfo.collider != null)
        //{
        //    //Debug.Log($"현재 검출된 것 : {hitInfo.collider.gameObject.name}");
        //}
        
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    isGround = true;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    isGround = false;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.12f, transform.position + Vector3.down * rayLength);
    }
}
