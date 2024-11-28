using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncedState : PlayerState
{
    //float forceOffset = 20f;
    float bounceDelayCounter;
    float bounceDelay = 0.6f;
    public BouncedState(PlayerController player) : base(player)
    {
        //animationIndex = (int)E_PlayeState.Bounced;
    }

    public override void Enter()
    {
        Debug.Log("Bounced 상태 진입");
        //player.view.SetAnimationTrigger(E_PlayeState.Bounced);
        player.view.PlayAnimation((int)E_PlayeState.Bounced);
        bounceDelayCounter = 0;
        player.rb.velocity = Vector3.zero;

        // 임시값들
        if(player.isGrounded)
        {
            //player.bouncedDir.x *= 1.0f;
            //player.bouncedDir.z *= 1.0f;
            player.bouncedDir.y += 0.1f;
        }
        else
        {
            player.bouncedDir.y += 0.2f;
        }

        //player.bouncedDir.x =  player.bouncedDir.x + player.bouncedDir.x * forceOffset;
        //Debug.Log($"Bounced Direction : {player.bouncedDir}");
        player.rb.AddForce(player.bouncedDir * player.bouncedForce, ForceMode.Impulse);
    }

    public override void Update()
    {
        //if (!player.view.IsAnimationFinished())// && player.rb.velocity.y > 0.1f ) // 무한 콩콩이 고쳐야함
        //{
        //    return;
        //}
            


    }

    public override void FixedUpdate()
    {
        if (bounceDelayCounter < bounceDelay)
        {
            bounceDelayCounter += Time.fixedDeltaTime;
            return;
        }
            

        //if (player.rb.velocity.sqrMagnitude < 0.1f) // 밀려나는 힘이 거의 사라졌을 때?
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

        //bounceDelayCounter += Time.fixedDeltaTime;

        //if(bounceDelayCounter < bounceDelay)
        //    return;

        //if (!player.view.IsAnimationFinished())
        //    return;

        //if (player.rb.velocity.sqrMagnitude < 0.1f) // 밀려나는 힘이 거의 사라졌을 때?
        //{
        //    if (player.isGrounded)
        //    {
        //        player.ChangeState(E_PlayeState.Idle);
        //    }
        //    else
        //    {
        //        player.ChangeState(E_PlayeState.Fall);
        //    }
        //}
    }

    public override void Exit()
    {
        Debug.Log("Bounced 상태 종료");
        bounceDelayCounter = 0;

    }
}
