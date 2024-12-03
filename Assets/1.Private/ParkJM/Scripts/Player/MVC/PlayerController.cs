using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IGrabbable
{
    public Rigidbody rb;
    private CapsuleCollider playerCollider;
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
    [SerializeField] public Transform camTransform;


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

    private float rayLength = 0.4f;

    public bool isGrounded;
    public bool isSlope;
    public bool onConveyor;
    public float groundAngleValue;
    public Vector3 perpAngle;

    [SerializeField] private Renderer _renderer;
    
    // 벽 체크
    [SerializeField] WallChecker wallChecker;

    // 상태
    [SerializeField] E_PlayeState curState;
    private PlayerState[] states = new PlayerState[(int)E_PlayeState.Size];

    // 물리 충돌 레이어
    private int obstacleLayer;
    private int playerLayer; //grab에 사용할것
    private int groundLayer;
    private int conveyorLayer;
    private int combinedGroundLayer;
    private int ignoreWallCheckLayer;

    // 컨베이어
    public Vector3 conveyorVel;

    public Transform grabPoint;
    private bool isAlreadyGrabbed;

    // 임시 변수
    public float bouncedForce = float.MinValue;
    public Vector3 bouncedDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = (Player)photonView.InstantiationData[0];
        model.playerNumber = player.GetPlayerNumber();
        view = GetComponent<PlayerView>();
        playerCollider = GetComponent<CapsuleCollider>();
        
        rayPoint1 = transform.Find("RayPoints/rayPointForward");
        rayPoint2 = transform.Find("RayPoints/rayPointLeft");
        rayPoint3 = transform.Find("RayPoints/rayPointRight");
        grabPoint = transform.Find("GrabPoint");
        wallChecker = GetComponentInChildren<WallChecker>();

        // 상태
        InitStates();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        if(player == PhotonNetwork.LocalPlayer)
        {
            _cam.SetForcePriority(10);
        }

        Vector3 colorValue = player.GetColor();
        Color color = new Color(colorValue.x, colorValue.y, colorValue.z);
        _renderer.material.color = color;

        curState = E_PlayeState.Idle;
        states[(int)curState].Enter();

        // 레이어 미리 캐싱
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        conveyorLayer = LayerMask.NameToLayer("Conveyor");
        ignoreWallCheckLayer = ~(1 << LayerMask.NameToLayer("WallCheck"));
        combinedGroundLayer = (1 << groundLayer) | (1 << conveyorLayer);
    }

    private void Update()
    {
        HandleCamInput();

        if (!photonView.IsMine)
            return;

        HandleMoveInputs();
        ControlJumpBuffer();
        states[(int)curState].Update();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        
        states[(int)curState].FixedUpdate();
        CheckGround();
        //CheckWall();

        //MoveOnConveyor();

        if (!isGrounded && curState != E_PlayeState.Bounced)
        {
            MoveInAir();
        }
    }

    private void LateUpdate()
    {
        _cam.RotY(rotVec.y);
        states[(int)curState].LateUpdate();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
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
            if (curState == E_PlayeState.Bounced)
                return;

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
        if (moveDir == Vector3.zero)
            return;


        Vector3 targetVelocity = moveDir * model.moveSpeedInAir;
        targetVelocity.y = rb.velocity.y;

        if (wallChecker.IsWallDetected)
        {
            targetVelocity.z = 0f;
            targetVelocity.x = 0f;
        }


        rb.velocity = targetVelocity;
        //Vector3 moveForce = targetVelocity - rb.velocity;
        //rb.AddForce(moveForce, ForceMode.VelocityChange);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.fixedDeltaTime * 15 // 수정필요
        );


            
    }

    private void InitStates()
    {
        // 상태
        states[(int)E_PlayeState.Idle] = new IdleState(this);
        states[(int)E_PlayeState.Run] = new RunState(this);
        states[(int)E_PlayeState.Jump] = new JumpState(this);
        states[(int)E_PlayeState.Fall] = new FallState(this);
        states[(int)E_PlayeState.Diving] = new DivingState(this);
        states[(int)E_PlayeState.FallingImpact] = new FallingImpact(this);
        states[(int)E_PlayeState.StandUp] = new StandUpState(this);
        states[(int)E_PlayeState.Bounced] = new BouncedState(this);
        states[(int)E_PlayeState.Grabbing] = new GrabbingState(this);
        states[(int)E_PlayeState.Grabbed] = new GrabbedState(this);
    }

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
        rotVec = RemoteInput.inputs[model.playerNumber].RotVec;
    }

    private void CheckGround()
    {
        bool rayhit1 = Physics.Raycast(rayPoint1.position, Vector3.down, out groundhit1, rayLength, combinedGroundLayer);
        bool rayhit2 = Physics.Raycast(rayPoint2.position, Vector3.down, out groundhit2, rayLength, combinedGroundLayer);
        bool rayhit3 = Physics.Raycast(rayPoint3.position, Vector3.down, out groundhit3, rayLength, combinedGroundLayer);

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

            if (((1 << chosenHit.collider.gameObject.layer) & (1 << conveyorLayer)) != 0)
            {
                onConveyor = true;
                conveyorVel = chosenHit.collider.gameObject.GetComponent<ConveyorBeltController>().conveyorVelocity;
                //Debug.Log($" 컨베이어 {conveyorVel}");
            }
            else
            {
                onConveyor = false;
            }

            // 각도 계산
            perpAngle = Vector3.Cross(chosenHit.normal,Vector3.up).normalized; //3D는 Perpendicular가 아닌 cross를 써야함, 외적을 계산해 수직벡터를 구하는 방식
            groundAngleValue = Vector3.Angle(chosenHit.normal, Vector3.up);

            isSlope = groundAngleValue > 0;

            Debug.DrawLine(chosenHit.point, chosenHit.point + chosenHit.normal, Color.blue); // 법선 벡터
            Debug.DrawLine(chosenHit.point, chosenHit.point + perpAngle, Color.red);        // 법선 벡터와 수직인 벡터
        }
        else
        {
            isSlope = false;
            onConveyor = false;
        }
    }

    //private void CheckWall()
    //{
    //    if (curState == E_PlayeState.Bounced) // + isGrounded 할 예정
    //        return;
    //    if (Physics.BoxCast(wallCheckPoint.position, wallCheckArea, wallCheckPoint.forward, out RaycastHit hit, wallCheckPoint.transform.rotation, wallCheckArea.z)) // Quaternion.identity 도 안됨
    //    {
    //        Debug.Log($"벽체크 감지된것 {hit.collider.name}");
    //        moveDir = Vector3.zero;
    //        //Vector3 slideDir = Vector3.Cross(hit.normal, Vector3.up).normalized;
    //        //rb.velocity = new Vector3(slideDir.x * rb.velocity.x, rb.velocity.y, slideDir.z * rb.velocity.z);
    //    }
    //}
    //private void DrawWallCheckGizmo()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckArea);
    //}

    public GameObject CheckGrabPoint()
    {
        Collider[] grabbedColliders;

        grabbedColliders = Physics.OverlapSphere(grabPoint.position, model.grabRadius, ignoreWallCheckLayer);

        if (grabbedColliders.Length > 0)
        {
            //IGrabbable grabbableObject = grabbedColliders[0].GetComponent<IGrabbable>();
            if(grabbedColliders[0].TryGetComponent<IGrabbable>(out IGrabbable grabbableObject))
            {
                //Debug.Log("grabbable 오브젝트 잡음");
                grabbableObject.OnGrabbedEnter();
                return grabbedColliders[0].gameObject;
            }
        }
        return null;
    }

    // 전체 다 잡기
    public GameObject CheckGrabPointAll()
    {
        Collider[] grabbedColliders = Physics.OverlapSphere(grabPoint.position, model.grabRadius);

        foreach (Collider col in grabbedColliders)
        {
            IGrabbable grabbableObject = col.GetComponent<IGrabbable>();
            if (grabbableObject != null)
            {
                grabbableObject.OnGrabbedEnter();
                return col.gameObject;
            }
        }

        return null;
    }

    public void MoveOnConveyor()
    {
        if (onConveyor)
        {
            rb.AddForce(conveyorVel - rb.velocity, ForceMode.VelocityChange);
        }
    }


    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(grabPoint.position, model.grabRadius);
        //DrawWallCheckGizmo();
    }

    private void SubscribeEvents()
    {
        model.OnPlayerJumped += HandleJumping;
        model.OnPlayerDove += HandleDiving;
        model.OnPlayerFloorImpacted += HandleFloorImpact;
        model.OnPlayerGrabbingObject += HandleGrabbing;
        model.OnPlayerGrabbed += HandleGrabbed;
        model.OnPlayerBounced += HandleBounced;
    }

    private void UnSubscribeEvents()
    {
        model.OnPlayerJumped -= HandleJumping;
        model.OnPlayerDove -= HandleDiving;
        model.OnPlayerFloorImpacted -= HandleFloorImpact;
        model.OnPlayerGrabbingObject -= HandleGrabbing;
        model.OnPlayerGrabbed -= HandleGrabbed;
        model.OnPlayerBounced -= HandleBounced;
    }

    private void HandleJumping()
    {
        // 사운드매니저, 이펙트 매니저 등의 동작 설정
        //EffectManager.Instance.PlayFX(transform.position, E_VFX.);
        //SoundManager.Instance.

    }

    private void HandleDiving()
    {
        // 사운드매니저, 이펙트 매니저 등의 동작 설정
    }

    private void HandleFloorImpact()
    {
        // 사운드매니저, 이펙트 매니저 등의 동작 설정
    }

    private void HandleGrabbing()
    {
        // 사운드매니저, 이펙트 매니저 등의 동작 설정
        EffectManager.Instance.PlayFX(grabPoint.transform.position, E_VFX.Grab, E_NetworkType.Public);
    }

    private void HandleGrabbed()
    {
        // 사운드매니저, 이펙트 매니저 등의 동작 설정
    }

    private void HandleBounced()
    {
        // 사운드매니저, 이펙트 매니저 등의 동작 설정
    }

    public void OnGrabbedEnter()
    {
        if(curState != E_PlayeState.Grabbed)
        {
            ChangeState(E_PlayeState.Grabbed);
        }
    }

    public void OnGrabbedLeave()
    {
        if(curState == E_PlayeState.Grabbed)
        {
            ChangeState(E_PlayeState.Idle);
        }
    }
}
