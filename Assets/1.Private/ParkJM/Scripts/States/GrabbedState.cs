using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedState : PlayerState
{
    // 애니메이션 index : 2
    public GrabbedState(PlayerController player) : base(player)
    {

    }

    public override void Enter()
    {
        Debug.Log("잡힘 상태 진입");
        player.view.SetBoolInGrabAnimation(2,true);
        // Todo : 잡혔을때 해줄 동작 : 바둥거리는 애니메이션 재생?
        player.rb.velocity = Vector3.zero;
    }

    public override void Update()
    {
        // 임시 잡힌상태 나가기
        if(player.jumpBufferCounter > 0f && player.isJumpable)
        {
            player.ChangeState(E_PlayeState.Jump);
        }
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        player.view.SetBoolInGrabAnimation(2, false);
        Debug.Log("잡힘 상태 해제");
    }
}
