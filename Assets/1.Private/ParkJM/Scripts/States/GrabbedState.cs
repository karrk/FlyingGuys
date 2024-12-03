using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedState : PlayerState
{
    public GrabbedState(PlayerController player) : base(player)
    {

    }

    public override void Enter()
    {
        player.view.SetBoolParameter(E_AniParameters.Struggling, true);
        player.model.InvokePlayerGrabbed();
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
        player.view.SetBoolParameter(E_AniParameters.Struggling, false);
    }
}
