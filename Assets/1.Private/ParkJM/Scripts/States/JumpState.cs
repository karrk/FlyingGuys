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
        player.isJumping = true;
        player.rb.AddForce(Vector3.up * player.model.jumpForce, ForceMode.Impulse);
    }

    public override void Update()
    {
        // 임시
        if (player.rb.velocity.y < -0.1f)
        {
            player.ChangeState(E_PlayeState.Fall);
        }

        //if (player.isGround)
        //{
        //    player.ChangeState(E_PlayeState.Idle);
        //}

        // Todo : fall 상태 전환
        //if(player.rb.velocity.y < 0)
        //{

        //}

    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        
        Debug.Log("Jump 종료");
    }
}
