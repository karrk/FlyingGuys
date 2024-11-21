using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 moveDir;
    [SerializeField] Vector3 rotVec;

    private Player player;
    [SerializeField] int playerNumber;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [SerializeField] CamController _cam;

    // 상태
    [SerializeField] E_PlayeState curState;
    private PlayerState[] states = new PlayerState[(int)E_PlayeState.Size];

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = (Player)photonView.InstantiationData[0];
        playerNumber = player.GetPlayerNumber();

        // 상태
        states[(int)E_PlayeState.Idle] = new IdleState(this);
        states[(int)(E_PlayeState.Run] = new RunState(this);
    }

    private void Start()
    {
        curState = E_PlayeState.Idle;
        states[(int)curState].Enter();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        states[(int)curState].Update();

        if (RemoteInput.inputs[playerNumber].jumpInput)
        {
            Debug.Log("점프 입력됨");
            JumpTemp();
            RemoteInput.inputs[playerNumber].jumpInput = false;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        states[(int)curState].FixedUpdate();

        moveDir = RemoteInput.inputs[playerNumber].MoveDir;
        rotVec = RemoteInput.inputs[playerNumber].RotVec;

        rb.velocity = moveDir.normalized * moveSpeed + Vector3.up * rb.velocity.y;
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

    private void JumpTemp()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
