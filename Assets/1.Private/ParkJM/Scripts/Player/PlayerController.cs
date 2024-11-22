using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    public Rigidbody rb;
    public Vector3 moveDir;
    public bool isJumping;
    public bool isDiving;
    [SerializeField] Vector3 rotVec;

    public PlayerModel model;

    private Player player;
    //[SerializeField] int playerNumber;
    //[SerializeField] float moveSpeed;
    //[SerializeField] float jumpForce;

    [SerializeField] CamController _cam;

    // 임시 바닥 탐지
    private float rayLength = 0.1f;
    public bool isGrounded;


    // 상태
    [SerializeField] E_PlayeState curState;
    private PlayerState[] states = new PlayerState[(int)E_PlayeState.Size];

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = (Player)photonView.InstantiationData[0];
        model.playerNumber = player.GetPlayerNumber();

        // 상태
        states[(int)E_PlayeState.Idle] = new IdleState(this);
        states[(int)E_PlayeState.Run] = new RunState(this);
        states[(int)E_PlayeState.Jump] = new JumpState(this);
        states[(int)E_PlayeState.Fall] = new FallState(this);
        states[(int)E_PlayeState.Diving] = new DivingState(this);
    }

    private void Start()
    {
        if(player == PhotonNetwork.LocalPlayer)
        {
            _cam.SetForcePriority(10);
        }

        curState = E_PlayeState.Idle;
        states[(int)curState].Enter();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        

        HandleMoveInputs();
        HandleCamInput();

        states[(int)curState].Update();

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



        //moveDir = RemoteInput.inputs[playerNumber].MoveDir;
        //rotVec = RemoteInput.inputs[playerNumber].RotVec;
        //rb.velocity = moveDir.normalized * model.moveSpeed + Vector3.up * rb.velocity.y;
    }

    private void LateUpdate()
    {
        _cam.RotY(rotVec.y);
    }


    public void ChangeState(E_PlayeState newState)
    {
        states[(int)curState].Exit();
        curState = newState;
        states[(int)curState].Enter();
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

    private void HandleCamInput()
    {
        Debug.Log(_cam.gameObject.name);
        rotVec = RemoteInput.inputs[model.playerNumber].RotVec;
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, rayLength);
        Debug.Log(hitInfo.collider.gameObject.name);
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
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }
}
