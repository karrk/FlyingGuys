using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingImpact : PlayerState
{
    public FallingImpact(PlayerController player) : base(player)
    {
        //animationIndex = (int)E_PlayeState.FallingImpact;
    }

    public override void Enter()
    {
        player.view.BroadCastTriggerParameter(E_AniParameters.FallingImpact);
        //player.view.PlayAnimation(animationIndex);
    }

    public override void Update()
    {
        if (!player.view.IsAnimationFinished())
            return;
        
        player.ChangeState(E_PlayeState.StandUp);

    }

    public override void Exit()
    {
    }
}
