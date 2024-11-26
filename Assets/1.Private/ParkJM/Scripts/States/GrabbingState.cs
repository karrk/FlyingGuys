using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbingState : PlayerState
{
    GameObject grabbedPlayer;
    float moveSpeedOnGrab;
    Vector3 targetVelocity;
    public GrabbingState(PlayerController player) : base(player)
    {
        moveSpeedOnGrab = player.model.moveSpeed * 0.2f;
    }
    public override void Enter()
    {
        Debug.Log("Grab 상태 진입");
    }

    public override void Update()
    {
        // Todo : grab을 한번 더 입력했을 때 쥐고있던 물체를 놔주고 idle상태로 전환
        // grabbedPlayer = null;
        if (RemoteInput.inputs[player.model.playerNumber].grabInput)
        {
            player.ChangeState(E_PlayeState.Idle);
        }
        // player.ChangeState(E_PlayeState.Idle);
    }

    public override void FixedUpdate()
    {
        if (grabbedPlayer == null)
        {
            Debug.Log("잡을 플레이어 없음");
            grabbedPlayer = player.CheckGrabPoint();
            Debug.Log("잡기시도");
            if (grabbedPlayer != null)
            {
                Debug.Log("플레이어 잡음");
                PushorPullGrabbedObject(grabbedPlayer);
            }
            else
            {
                Debug.Log("플레이어 잡지못함");
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Grab 상태 해제");
    }
    
    private void PushorPullGrabbedObject(GameObject grabbedObject)
    {
        if (player.isSlope) // 오를 수 있는 maxAngle 설정을 할지는 추후에
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(player.moveDir, player.chosenHit.normal).normalized;

            targetVelocity = slopeDirection * moveSpeedOnGrab;
            player.rb.velocity = targetVelocity;

            //targetVelocity = player.moveDir * player.perpAngle * player.model.moveSpeed;
        }
        else
        {
            targetVelocity = player.moveDir * moveSpeedOnGrab;
            targetVelocity.y = player.rb.velocity.y;

            player.rb.velocity = targetVelocity;
        }

        grabbedObject.gameObject.GetComponent<Rigidbody>().velocity = targetVelocity;

        if (player.moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.moveDir);
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * 15 // 수정필요
            );
        }
    }
}
