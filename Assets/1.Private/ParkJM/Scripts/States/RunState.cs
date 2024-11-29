using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    Vector3 targetVelocity;
    public RunState(PlayerController player) : base(player)
    {
        //animationIndex = (int)E_PlayeState.Run;
    }

    public override void Enter()
    {
        Debug.Log("Run 진입");
        //player.view.SetBoolParameter(E_AniParameters.Running, true);
        player.view.BroadCastBoolParameter(E_AniParameters.Running, true);
        //player.view.SetAnimationBoolTrue(E_PlayeState.Run);
        //player.view.PlayRun();
        //player.view.PlayAnimation(animationIndex);
    }

    public override void Update()
    {
        if (player.jumpBufferCounter > 0f && player.isJumpable)
        {
            player.ChangeState(E_PlayeState.Jump);
        }
        else if(!player.isGrounded)
        {
            player.ChangeState(E_PlayeState.Fall);
        }
        //if (RemoteInput.inputs[player.model.playerNumber].jumpInput && player.isJumpable)
        //{
        //    player.ChangeState(E_PlayeState.Jump);
        //}

        else if (player.moveDir.sqrMagnitude < 0.1f) //== Vector3.zero)
        {
            player.ChangeState(E_PlayeState.Idle);
        }
        else if (RemoteInput.inputs[player.model.playerNumber].grabInput)
        {
            player.ChangeState(E_PlayeState.Grabbing);
        }

    }

    public override void FixedUpdate()
    {
        Run();
        LookForward();
    }

    public override void LateUpdate()
    {
        //LookForward();
    }

    public override void Exit()
    {
        Debug.Log("Run 종료");
        player.view.BroadCastBoolParameter(E_AniParameters.Running, false);
        //player.view.SetBoolParameter(E_AniParameters.Running, false);
        //player.view.SetAnimationBoolFalse(E_PlayeState.Run);
        //player.view.StopRun();
        targetVelocity = Vector3.zero;
    }

    private void Run()
    {
        if (player.isSlope) // 오를 수 있는 maxAngle 설정을 할지는 추후에
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(player.moveDir, player.chosenHit.normal).normalized;

            targetVelocity = slopeDirection * player.model.moveSpeed;
            player.rb.velocity = targetVelocity;

            //targetVelocity = player.moveDir * player.perpAngle * player.model.moveSpeed;
        }
        else
        {
            targetVelocity = player.moveDir * player.model.moveSpeed;
            targetVelocity.y = player.rb.velocity.y;

            player.rb.velocity = targetVelocity;
        }



        //player.rb.velocity = player.moveDir * player.model.moveSpeed + Vector3.up * player.rb.velocity.y;
    }

    private void LookForward()
    {
        if (player.moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.moveDir);
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * 15 // 수정필요
            );
        }
    }
}
