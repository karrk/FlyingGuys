using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    private Vector3 dir;
    private Vector3 camForward;
    private Vector3 camRight;

    public RunState(PlayerController player) : base(player)
    {

    }

    public override void Enter()
    {
        Debug.Log("Run 진입");
    }

    public override void Update()
    {
        if (RemoteInput.inputs[player.model.playerNumber].jumpInput && !player.isJumping)
        {
            player.ChangeState(E_PlayeState.Jump);
        }

        if (player.moveDir == Vector3.zero)
        {
            player.ChangeState(E_PlayeState.Idle);
        }
    }

    public override void FixedUpdate()
    {
        UpdateCamDir();
        Run();
    }

    public override void Exit()
    {
        Debug.Log("Run 종료");
    }

    private void UpdateCamDir()
    {
        camForward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;
    }

    private void Run()
    {
        dir = (player.moveDir.z * camForward + player.moveDir.x * camRight).normalized;


        player.rb.velocity = player.moveDir.normalized * player.model.moveSpeed + Vector3.up * player.rb.velocity.y;

        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            player.rb.transform.rotation = Quaternion.Slerp(player.rb.transform.rotation, targetRotation, Time.deltaTime * 10);
        }
    }
}
