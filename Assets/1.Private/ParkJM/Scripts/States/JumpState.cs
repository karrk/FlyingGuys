using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerController player) : base(player)
    {

    }

    public override void Enter()
    {
        Debug.Log("Jump 진입");
        
        player.rb.AddForce(Vector3.up * player.model.jumpForce, ForceMode.Impulse);
        player.isJumpable = false;
    }

    public override void Update()
    {
        // 임시
        if (player.rb.velocity.y < 0f && !player.isGrounded)
        {
            player.ChangeState(E_PlayeState.Fall);
        }

        if (RemoteInput.inputs[player.model.playerNumber].divingInput)
        {
            player.ChangeState(E_PlayeState.Diving);
        }



        // Todo : fall 상태 전환
        //if(player.rb.velocity.y < 0)
        //{

        //}

    }

    public override void FixedUpdate()
    {



        //if (player.isGrounded)
        //{
        //    player.isJumpable = true;
        //    player.ChangeState(E_PlayeState.Idle);
        //}
    }

    public override void Exit()
    {
        Debug.Log("Jump 종료");
    }
}
