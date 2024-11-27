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
    private Coroutine grabCheckRoutine;

    public GrabbingState(PlayerController player) : base(player)
    {
        moveSpeedOnGrab = player.model.moveSpeed * 0.2f;
    }
    public override void Enter()
    {
        Debug.Log("Grab 상태 진입");
        grabbedObject = null;
        grabSearchCounter = 0;

        if(grabCheckRoutine == null)
        {
            grabCheckRoutine = player.StartCoroutine(CheckGrabPointRoutine());
        }
    }

    public override void Update()
    {
        if (RemoteInput.inputs[player.model.playerNumber].grabInput)
        {
            player.ChangeState(E_PlayeState.Idle);
            return;
        }

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
    }

    public override void FixedUpdate()
    {

        ApplyMovement();

        if(grabbedObject != null)
        {
            PushOrPullGrabbedObject(grabbedObject);
        }


        //if (player.isSlope)
        //{
        //    Vector3 slopeDirection = Vector3.ProjectOnPlane(player.moveDir, player.chosenHit.normal).normalized;

        //    targetVelocity = slopeDirection * moveSpeedOnGrab;
        //    player.rb.velocity = targetVelocity;
        //}
        //else
        //{
        //    targetVelocity = player.moveDir * moveSpeedOnGrab;
        //    targetVelocity.y = player.rb.velocity.y;

        //    player.rb.velocity = targetVelocity;
        //}

        //if (grabbedObject == null)
        //{
        //    grabbedObject = player.CheckGrabPoint();

        //    if(grabbedObject != null)
        //    {
        //        PushOrPullGrabbedObject(grabbedObject);
        //    }
        //}

        //if(grabbedObject == null)

        //curGrabbedObject = player.CheckGrabPoint();

        //if (grabbedObject != null && curGrabbedObject == null)
        //{
        //    Debug.Log("잡은 물체가 범위를 벗어나서 Idle 상태로 전환");
        //    if (grabbedObject is IGrabbable)
        //        grabbedObject.OnGrabbedLeave();

        //        grabbedObject.gameObject
        //    player.ChangeState(E_PlayeState.Idle);
        //    return;
        //}

        //grabbedObject = curGrabbedObject;


        //if (grabbedObject != null)
        //{
        //    Debug.Log(" 잡음");
        //    PushOrPullGrabbedObject(grabbedObject);
        //}
    }

    public override void Exit()
    {
        Debug.Log("Grab 상태 해제");
        grabbedObject = null;
        grabSearchCounter = 0f;

        if(grabCheckRoutine != null)
        {
            player.StopCoroutine(grabCheckRoutine);
            grabCheckRoutine = null;
        }
    }

    private void ApplyMovement()
    {
        Vector3 targetVelocity;
        if (player.isSlope)
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(player.moveDir, player.chosenHit.normal).normalized;
            targetVelocity = slopeDirection * moveSpeedOnGrab;
        }
        else
        {
            targetVelocity = player.moveDir * moveSpeedOnGrab;
            targetVelocity.y = player.rb.velocity.y;
        }
        player.rb.velocity = targetVelocity;
    }

    private IEnumerator CheckGrabPointRoutine()
    {
        while (true)
        {
            GameObject detectedObject = player.CheckGrabPoint();
            if (detectedObject != null && detectedObject != grabbedObject)
            {
                grabbedObject = detectedObject;
                Debug.Log($"새로운 GrabbedObject: {grabbedObject.name}");
            }
            else if (detectedObject == null)
            {
                grabbedObject = detectedObject;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void PushOrPullGrabbedObject(GameObject grabbedObject)
    {
        Rigidbody grabbedObjectRb = grabbedObject.gameObject.GetComponent<Rigidbody>();

        if (grabbedObjectRb == null || grabbedObjectRb.isKinematic)
        {
            Debug.LogWarning("rigidbody가 없거나 Kinematic 임");
            return;
        }

        Vector3 camForward = player.camTransform.forward.normalized;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 moveDir = player.moveDir; //moveDir은 이미 정규화된 값

        // 내적 계산 후 push인지 pull인지 결정
        float dotProduct = Vector3.Dot(camForward, moveDir);
        //Debug.Log($"벡터 내적 : {dotProduct}");

        if (dotProduct > 0f)
        {
            // 밀기
            Debug.Log("밀기");
            grabbedObjectRb.velocity = camForward * player.model.grabForce;
            // 밀기 애니메이션 재생
            // 이미 재생중이라면 애니메이션 재생x 밀기 당기기 바꿀때만 재생
            //grabbedObjectRb.AddForce(moveDir * player.model.grabForce, ForceMode.Force);
        }
        else if(dotProduct < 0f)
        {
            // 당기기
            Debug.Log("당기기");
            grabbedObjectRb.velocity = -camForward * player.model.grabForce;
            //grabbedObjectRb.AddForce(-moveDir * player.model.grabForce, ForceMode.Force);
        }




        //grabbedObjectRb.AddForce(player.moveDir * player.model.grabForce, ForceMode.Force); // 지속적으로 해주는거라 이러면 안됨

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
