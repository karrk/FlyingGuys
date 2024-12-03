using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivingState : PlayerState
{
    public DivingState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        player.view.BroadCastTriggerParameter(E_AniParameters.Diving);
        player.model.InvokePlayerDove();

        //Vector3 diveSpeed = player.moveDir * player.model.divingForce;
        //diveSpeed.y = 5.0f; // 임시 y축 힘

        // 기존 힘 유지 방식, 점프와 다이빙을 연속해 누를경우 슈퍼점프가 됨
        //Vector3 targetVelocity = player.rb.velocity;
        //targetVelocity.y += player.model.divingForce;
        //player.rb.AddForce(targetVelocity, ForceMode.Impulse);

        // 기존 힘의 방향만 사용 방식, 방향만 사용
        //Vector3 targetVelocity = player.rb.velocity.normalized;
        //targetVelocity *= player.model.divingForce;
        ////if (!player.isGrounded)
        ////    targetVelocity.y = 0f;

        //player.rb.AddForce(targetVelocity, ForceMode.Impulse);

        // 기존 힘 초기화, 다이빙 힘만 적용하는 방식
        Vector3 targetVelocity = player.rb.velocity.normalized;
        player.rb.velocity = Vector3.zero;
        targetVelocity += Vector3.up * player.model.divingForce;
        player.rb.AddForce(targetVelocity, ForceMode.VelocityChange);
        

    }

    public override void Update()
    {
        // Todo : 다이빙 애니메이션이 끝나면 Fall로 전환

        if (!player.view.IsAnimationFinished()) // 없는편이 더 자연스럽다
            return;

        if (player.isGrounded) // player.rb.velocity.y  < 0.1f ||
        {
            player.ChangeState(E_PlayeState.FallingImpact);
            return;
            //player.transform.rotation = Quaternion.Euler(0f, player.transform.rotation.y, player.transform.rotation.z);

        }
            
    }

    public override void FixedUpdate()
    {
        // 다이빙중 감속
        Vector3 velocity = player.rb.velocity;
        //velocity.x *= 0.98f;
        //velocity.z *= 0.98f;
        velocity.y *= 0.98f;
        player.rb.velocity = velocity;
    }

    public override void Exit()
    {
        //player.view.isAniFinished = false;
    }
}
