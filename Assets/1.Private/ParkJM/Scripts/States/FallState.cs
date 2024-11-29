using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerController player) : base(player)
    {
        //animationIndex = (int)E_PlayeState.Fall;
    }

    public override void Enter()
    {
        Debug.Log("Fall 진입");
        player.view.BroadCastTriggerParameter(E_AniParameters.Falling);
        //player.view.SetAnimationTrigger(E_PlayeState.Fall);
        //player.view.PlayAnimation(animationIndex);
    }

    public override void Update()
    {
        if (RemoteInput.inputs[player.model.playerNumber].divingInput)
        {
            player.ChangeState(E_PlayeState.Diving);
        }

        // Todo : 점핑 발판을 받았을 때 어떻게 할것인지 처리
    }

    public override void FixedUpdate()
    {
        if (player.isGrounded)
        {
            player.ChangeState(E_PlayeState.Idle);
        }
    }

    public override void Exit()
    {
        Debug.Log("Fall 종료");
    }
}
