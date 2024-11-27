using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbingState : PlayerState
{
    GameObject grabbedObject;
    float moveSpeedOnGrab;
    Vector3 targetVelocity;
    public GrabbingState(PlayerController player) : base(player)
    {
        moveSpeedOnGrab = player.model.moveSpeed * 0.2f;
    }
    public override void Enter()
    {
        Debug.Log("Grab 상태 진입");
        grabbedObject = player.CheckGrabPoint();
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
        if (player.isSlope)
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(player.moveDir, player.chosenHit.normal).normalized;

            targetVelocity = slopeDirection * moveSpeedOnGrab;
            player.rb.velocity = targetVelocity;
        }
        else
        {
            targetVelocity = player.moveDir * moveSpeedOnGrab;
            targetVelocity.y = player.rb.velocity.y;

            player.rb.velocity = targetVelocity;
        }

        grabbedObject = player.CheckGrabPoint();

        if (grabbedObject != null)
        {
            Debug.Log(" 잡음");
            PushorPullGrabbedObject(grabbedObject);
        }
    }

    public override void Exit()
    {
        Debug.Log("Grab 상태 해제");
        grabbedObject = null;
    }
    
    private void PushorPullGrabbedObject(GameObject grabbedObject)
    {
        Rigidbody grabbedObjectRb = grabbedObject.gameObject.GetComponent<Rigidbody>();
        if (grabbedObjectRb == null)
            return;
        Debug.Log("물건 움직임");
        grabbedObjectRb.velocity = player.moveDir * 2f;


        //grabbedObject.gameObject.GetComponent<Rigidbody>().velocity = targetVelocity;
        //Debug.Log($"TargetVelocity : {targetVelocity}");
        //Debug.Log($"잡힌 오브젝트 이름 : {grabbedObject.gameObject.name}");


        //if (player.moveDir != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(player.moveDir);
        //    targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        //    player.transform.rotation = Quaternion.Slerp(
        //        player.transform.rotation,
        //        targetRotation,
        //        Time.fixedDeltaTime * 15 // 수정필요
        //    );
        //}
    }
}
