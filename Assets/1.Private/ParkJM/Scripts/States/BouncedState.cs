using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncedState : PlayerState
{
    //float forceOffset = 20f;
    float bounceDelayCounter;
    float bounceDelay = 0.03f;
    public BouncedState(PlayerController player) : base(player)
    {

    }

    public override void Enter()
    {
        Debug.Log("Bounced 상태 진입");
        bounceDelayCounter = 0;
        player.rb.velocity = Vector3.zero;

        // 임시값들
        if(player.isGrounded)
        {
            player.bouncedDir.x *= 1.6f;
            player.bouncedDir.z *= 1.6f;
            player.bouncedDir.y += 0.1f;
        }
        else
        {
            player.bouncedDir.y += 0.2f;
        }

        //player.bouncedDir.x =  player.bouncedDir.x + player.bouncedDir.x * forceOffset;
        Debug.Log($"Bounced Direction : {player.bouncedDir}");
        player.rb.AddForce(player.bouncedDir * player.bouncedForce, ForceMode.Impulse);
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        bounceDelayCounter += Time.fixedDeltaTime;

        if(bounceDelayCounter < bounceDelay)
            return;

        if (player.rb.velocity.sqrMagnitude < 0.1f) // 밀려나는 힘이 거의 사라졌을 때?
        {
            if (player.isGrounded)
            {
                player.ChangeState(E_PlayeState.Idle);
            }
            else
            {
                player.ChangeState(E_PlayeState.Fall);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Bounced 상태 종료");
        bounceDelayCounter = 0;

    }
}
