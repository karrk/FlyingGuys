using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerController player) : base(player)
    {
        //animationIndex = (int)E_PlayeState.Jump;
    }

    public override void Enter()
    {
        Debug.Log("Jump 진입");
        player.view.BroadCastTriggerParameter(E_AniParameters.Jumping);
        //player.view.SetAnimationTrigger(E_PlayeState.Jump);
        //player.view.PlayAnimation(animationIndex);

        Jump();
    }

    public override void Update()
    {
        // 임시
        if (player.rb.velocity.y < 0f && !player.isGrounded)
        {
            Debug.Log("jump에서 Fall로 전환");
            player.ChangeState(E_PlayeState.Fall);
        }
        else if (RemoteInput.inputs[player.model.playerNumber].divingInput)
        {
            player.ChangeState(E_PlayeState.Diving);
        }

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
        player.rb.AddForce(Vector3.up * player.model.jumpForce, ForceMode.Impulse);
        Debug.Log("점프 힘을 줌");
        
        player.isJumpable = false;
        Debug.Log("isJumpable을 false로 바꿈");
    }

    public override void Exit()
    {
        Debug.Log("Jump 종료");
    }
}
