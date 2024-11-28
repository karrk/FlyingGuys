using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivingState : PlayerState
{
    public DivingState(PlayerController player) : base(player)
    {
        animationIndex = (int)E_PlayeState.Diving;
    }

    public override void Enter()
    {
        Debug.Log("Diving 진입");
        //player.view.SetAnimationTrigger(E_PlayeState.Diving);
        player.view.PlayAnimation(animationIndex);
        // player.isDiving = true;

        // Todo : 다이빙 구현
        // 점프를 하지않고도 다이빙을 할 수 있음
        // 기본적으로 y축으로도 힘을 가해줘야함
        // 이동 방향이 있었을 경우 그쪽으로도 약간의 힘을 가해주어야 함

        //Vector3 diveSpeed = player.moveDir * player.model.divingForce;
        //diveSpeed.y = 5.0f; // 임시 y축 힘

        // 기존 힘 유지 방식, 점프와 다이빙을 연속해 누를경우 슈퍼점프가 됨
        //Vector3 targetVelocity = player.rb.velocity;
        //targetVelocity.y += player.model.divingForce;
        //player.rb.AddForce(targetVelocity, ForceMode.Impulse);

        // 기존 힘의 방향만 사용 방식, 방향만 사용
        Vector3 targetVelocity = player.rb.velocity.normalized;
        targetVelocity *= player.model.divingForce;
        if (!player.isGrounded)
            targetVelocity.y = 0f;

        player.rb.AddForce(targetVelocity, ForceMode.Impulse);


    }

    public override void Update()
    {
        // Todo : 다이빙 애니메이션이 끝나면 Fall로 전환

        //if (!player.view.IsAnimationFinished()) // 없는편이 더 자연스럽다
        //    return;

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
        velocity.x *= 0.98f;
        velocity.z *= 0.98f;
        player.rb.velocity = velocity;
    }

    public override void Exit()
    {
        Debug.Log("Diving 종료");
        //player.view.isAniFinished = false;
    }
}
