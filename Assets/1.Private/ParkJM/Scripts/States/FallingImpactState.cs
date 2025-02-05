using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingImpact : PlayerState
{
    public FallingImpact(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        player.view.BroadCastTriggerParameter(E_AniParameters.FallingImpact);
        player.model.InvokePlayerFloorImpacted();
    }

    public override void Update()
    {
        if (!player.view.IsAnimationFinished())
            return;
        
        player.ChangeState(EPlayerState.StandUp);
    }

    public override void Exit()
    {
    }
}
