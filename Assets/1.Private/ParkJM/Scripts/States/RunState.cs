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
        //player.view.SetBoolParameter(E_AniParameters.Running, true);

        //player.view.BroadCastBoolParameter(E_AniParameters.Running, true);
        player.view.SetBoolParameter(E_AniParameters.Running, true);



        //player.view.SetAnimationBoolTrue(E_PlayeState.Run);
        //player.view.PlayRun();
        //player.view.PlayAnimation(animationIndex);
    }

    public override void Update()
    {
        if (player.jumpBufferCounter > 0f && player.isJumpable)
        {
            player.ChangeState(EPlayerState.Jump);
        }
        else if(!player.isGrounded)
        {
            player.ChangeState(EPlayerState.Fall);
        }
        //if (RemoteInput.inputs[player.model.playerNumber].jumpInput && player.isJumpable)
        //{
        //    player.ChangeState(E_PlayeState.Jump);
        //}

        else if (player.moveDir.sqrMagnitude < 0.1f) //== Vector3.zero)
        {
            player.ChangeState(EPlayerState.Idle);
        }
        else if (RemoteInput.inputs[player.model.playerNumber].grabInput)
        {
            player.ChangeState(EPlayerState.Grabbing);
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
        //player.view.BroadCastBoolParameter(E_AniParameters.Running, false);
        player.view.SetBoolParameter(E_AniParameters.Running, false);



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
            //player.rb.velocity = targetVelocity;

            //targetVelocity = player.moveDir * player.perpAngle * player.model.moveSpeed;
        }
        // ddd
        else
        {
            targetVelocity = player.moveDir * player.model.moveSpeed;
            targetVelocity.y = player.rb.velocity.y;

            //player.rb.velocity = targetVelocity;
        }

        // 여기서 앞서 계산한 targetVelocity방향에 따른 속도 증감 처리
        // 플레이어의 최대 속도는 제한되어야함, 컨베이어 벨트 위에서 최대속도를 넘어설수도 있도록 속도처리도 될 수 있게

        //if (targetVelocity.sqrMagnitude > player.model.maxSpeed * player.model.maxSpeed)
        //{
        //    Debug.Log("제한됨"); // 어지간한 상황에선 안나올듯
        //    targetVelocity = targetVelocity.normalized * player.model.maxSpeed;
        //}

        Vector3 moveForce = targetVelocity - player.rb.velocity;

        if (player.onConveyor)
        {
            player.rb.AddForce(moveForce + player.conveyorVel, ForceMode.VelocityChange);
        }
        else
        {
            player.rb.AddForce(moveForce, ForceMode.VelocityChange);
        }

        //Debug.Log(player.rb.velocity.sqrMagnitude);
        //moveForce = Vector3.ClampMagnitude(moveForce, player.model.maxSpeed);
        
       
        
    }
    //player.rb.velocity = player.moveDir * player.model.moveSpeed + Vector3.up * player.rb.velocity.y;
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
