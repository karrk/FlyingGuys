using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerController player) : base(player)
    {

    }

    public override void Enter()
    {
        Debug.Log("Fall 진입");
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
        player.isJumpable = true;
    }
}
