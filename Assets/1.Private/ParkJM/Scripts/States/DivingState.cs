using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivingState : PlayerState
{
    public DivingState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        Debug.Log("Diving 진입");
        player.isDiving = true;
        player.rb.AddForce(player.moveDir * player.model.divingForce, ForceMode.Impulse);
    }

    public override void Update()
    {
        // Todo : 다이빙 애니메이션이 끝나면 Fall로 전환
        if(player.rb.velocity.y  == 0.1f)
            player.ChangeState(E_PlayeState.Fall);
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Diving 종료");
    }
}
