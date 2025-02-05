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
        Debug.Log("점프진입");
        player.view.BroadCastTriggerParameter(E_AniParameters.Jumping);
        player.model.InvokePlayerJumped();
        if(player.isJumpable)
            Jump();
        
    }

    public override void Update()
    {
        // 임시
        if (player.rb.velocity.y < 0.1f)// && !player.isGrounded)
        {
            player.ChangeState(EPlayerState.Fall);
        }
        else if (RemoteInput.inputs[player.model.playerNumber].divingInput)
        {
            player.ChangeState(EPlayerState.Diving);
        }
        //else if(player.isGrounded)
        //{
        //    player.ChangeState(E_PlayeState.Idle);
        //}

        //else if(player.isGrounded)
        {
            // 추가됨, 활성화하면 슈퍼점프 버그
            //player.ChangeState(E_PlayeState.Idle);
        }

        //Debug.Log($"점프 velocity: {player.rb.velocity.y}");


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

    private void Jump()
    {
        Debug.Log("점프");
        Vector3 targetVel = player.rb.velocity;
        targetVel.y = player.model.jumpForce;
        player.rb.velocity = targetVel;
        //player.rb.AddForce(Vector3.up * player.model.jumpForce, ForceMode.Impulse);
        player.isJumpable = false;
    }

    public override void Exit()
    {
        
    }
}
