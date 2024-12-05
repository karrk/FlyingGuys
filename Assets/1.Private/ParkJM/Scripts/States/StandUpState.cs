using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandUpState : PlayerState
{
    public StandUpState(PlayerController player) : base(player)
    {
        //animationIndex = (int)E_PlayeState.StandUp;
    }

    public override void Enter()
    {
        player.view.BroadCastTriggerParameter(E_AniParameters.StandingUp);
        //player.view.PlayAnimation(animationIndex);
    }

    public override void Update()
    {
        if (!player.view.IsAnimationFinished())
            return;
        //if(player.isGrounded)
            player.ChangeState(E_PlayeState.Idle);
    }

    public override void Exit()
    {
    }
}
