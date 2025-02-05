using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncedState : PlayerState
{
    float bounceDelayCounter;
    float bounceDelay = 0.6f;
    public BouncedState(PlayerController player) : base(player)
    {
       
    }

    public override void Enter()
    {
        player.view.BroadCastTriggerParameter(E_AniParameters.Bouncing);
        player.model.InvokePlayerBounced();
        bounceDelayCounter = 0;
        player.rb.velocity = Vector3.zero;

       
        if(player.isGrounded)
        {
            // 약간은 튀게 해주어 정상적으로 힘이 작용할 수 있도록함
            player.bouncedDir.y += 0.1f;
        }
        else
        {
            player.bouncedDir.y += 0.2f;
        }

        player.rb.AddForce(player.bouncedDir * player.bouncedForce, ForceMode.Impulse);
    }

    public override void Update()
    {
        if (bounceDelayCounter < bounceDelay)
        {
            bounceDelayCounter += Time.deltaTime;
            return;
        }

        //if (player.rb.velocity.sqrMagnitude < 0.1f) // 밀려나는 힘이 거의 사라졌을 때?
        {
            if (player.isGrounded)
            {
                player.ChangeState(EPlayerState.Idle);
            }
            else
            {
                player.ChangeState(EPlayerState.Fall);
            }
        }
    }

    public override void FixedUpdate()
    {
        //player.MoveOnConveyor();
        //if (bounceDelayCounter < bounceDelay)
        //{
        //    bounceDelayCounter += Time.fixedDeltaTime;
        //    return;
        //}


        ////if (player.rb.velocity.sqrMagnitude < 0.1f) // 밀려나는 힘이 거의 사라졌을 때?
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
        bounceDelayCounter = 0;
    }
}
