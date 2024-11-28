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
        Vector3 diveSpeed = player.moveDir * player.model.divingForce;
        diveSpeed.y = 1.0f; // 임시 y축 힘
        //player.transform.rotation = Quaternion.Euler(80f, player.transform.rotation.y, player.transform.rotation.z);
        player.rb.AddForce(diveSpeed, ForceMode.Impulse);
    }

    public override void Update()
    {
        // Todo : 다이빙 애니메이션이 끝나면 Fall로 전환

        if (!player.view.IsAnimationFinished())
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
        velocity.x *= 0.98f; // X축 감속
        velocity.z *= 0.98f; // Z축 감속
        player.rb.velocity = velocity;
    }

    public override void Exit()
    {
        Debug.Log("Diving 종료");
        //player.view.isAniFinished = false;
    }
}
