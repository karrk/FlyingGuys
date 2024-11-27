using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GrabbingState : PlayerState
{
    private GameObject grabbedObject;
    private GameObject curGrabbedObject;
    private float moveSpeedOnGrab;
    private Vector3 targetVelocity;
    private float grabSearchTime = 2f;
    private float grabSearchCounter;

    public GrabbingState(PlayerController player) : base(player)
    {
        moveSpeedOnGrab = player.model.moveSpeed * 0.2f;
    }
    public override void Enter()
    {
        Debug.Log("Grab 상태 진입");
        //grabbedObject = player.CheckGrabPoint();
        grabbedObject = null;
        curGrabbedObject = null;
        grabSearchCounter = 0;
    }

    public override void Update()
    {
        // Todo : grab을 한번 더 입력했을 때 쥐고있던 물체를 놔주고 idle상태로 전환
        // grabbedPlayer = null;

        if (grabbedObject == null)
        {
            grabSearchCounter += Time.deltaTime;
            if (grabSearchCounter >= grabSearchTime)
            {
                player.ChangeState(E_PlayeState.Idle);
                return;
            }
        }
        else
        {
            grabSearchCounter = 0f;
        }

        if (RemoteInput.inputs[player.model.playerNumber].grabInput)
        {
            player.ChangeState(E_PlayeState.Idle);
            return;
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

        curGrabbedObject = player.CheckGrabPoint();

        if (grabbedObject != null && curGrabbedObject == null)
        {
            Debug.Log("잡은 물체가 범위를 벗어나서 Idle 상태로 전환");
            player.ChangeState(E_PlayeState.Idle);
            return;
        }

        grabbedObject = curGrabbedObject;


        if (grabbedObject != null)
        {
            Debug.Log(" 잡음");
            PushOrPullGrabbedObject(grabbedObject);
        }
    }

    public override void Exit()
    {
        Debug.Log("Grab 상태 해제");
        grabbedObject = null;
        grabSearchCounter = 0f;
    }
    
    private void PushOrPullGrabbedObject(GameObject grabbedObject)
    {
        Rigidbody grabbedObjectRb = grabbedObject.gameObject.GetComponent<Rigidbody>();
        if (grabbedObjectRb == null)
            return;
        Debug.Log("물건 움직임");
        grabbedObjectRb.AddForce(player.moveDir * player.model.grabForce, ForceMode.Force);
        // 잡았다가 놓치는경우
        //if (grabbedObjectRb == null)
        //{
        //    player.ChangeState(E_PlayeState.Idle);
        //}

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
