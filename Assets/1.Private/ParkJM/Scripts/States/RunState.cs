using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
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
        Run();
    }

    public override void Exit()
    {
        Debug.Log("Run 종료");
    }

    private void Run()
    {
        player.rb.velocity = player.moveDir.normalized * player.model.moveSpeed + Vector3.up * player.rb.velocity.y;
    }
}
