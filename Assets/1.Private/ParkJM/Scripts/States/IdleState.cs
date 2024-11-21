using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player)
    {

    }

    public override void Enter()
    {
        Debug.Log("Idle 진입");
    }

    public override void Update()
    {

        if (RemoteInput.inputs[player.model.playerNumber].jumpInput && !player.isJumping)
        {
            player.ChangeState(E_PlayeState.Jump);
        }
        else if (player.moveDir != Vector3.zero)
        {
            player.ChangeState(E_PlayeState.Run);
        }


                

        // Todo : fall 상태 전환
        //if(player.rb.velocity.y < 0)
        //{

        //}

        //moveDir = RemoteInput.inputs[playerNumber].MoveDir;
        //rotVec = RemoteInput.inputs[playerNumber].RotVec;
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Idle 종료");
    }
}
