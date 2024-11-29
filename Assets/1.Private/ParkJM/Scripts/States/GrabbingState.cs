using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GrabbingState : PlayerState
{
    private GameObject grabbedObject;
    //private GameObject curGrabbedObject;
    private float moveSpeedOnGrab;
    private Vector3 targetVelocity;
    private float grabSearchTime = 1.0f;
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
        player.view.SetBoolParameter(E_AniParameters.Pushing, true);
        //player.view.SetBoolInGrabAnimation(0, true);
        //player.view.UpSpine();

        if(grabCheckRoutine == null)
        {
            grabCheckRoutine = player.StartCoroutine(CheckGrabPointRoutine());
        }
    }

    public override void Update()
    {
        if (!RemoteInput.inputs[player.model.playerNumber].grabInput)
        {
            ReleaseGrabbedObject();
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
    }

    public override void LateUpdate()
    {
        player.view.UpSpine();
    }

    public override void Exit()
    {
        Debug.Log("Grab 상태 해제");
        ReleaseGrabbedObject();
        grabSearchCounter = 0f;
        player.view.SetBoolParameter(E_AniParameters.Pushing, false);
        player.view.SetBoolParameter(E_AniParameters.Pulling, false);
        //player.view.SetBoolInGrabAnimation(0, false);
        //player.view.SetBoolInGrabAnimation(1, false);

        if (grabCheckRoutine != null)
        {
            player.StopCoroutine(grabCheckRoutine);
            grabCheckRoutine = null;
        }
    }

    private void ReleaseGrabbedObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.GetComponent<IGrabbable>().OnGrabbedLeave();
            grabbedObject = null;
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

            if (detectedObject != grabbedObject)
            {
                // 잡힌 오브젝트가 바뀌거나, 잡힌 오브젝트가 범위를 벗어났을 때
                if (grabbedObject != null) 
                {
                    // 이전에 잡힌 오브젝트가 있을 경우
                    grabbedObject.GetComponent<IGrabbable>().OnGrabbedLeave();
                    Debug.Log($"GrabbedObject 해제: {grabbedObject.name}");
                }

                grabbedObject = detectedObject;

                if (grabbedObject != null) 
                {
                    // 새롭게 잡힌 오브젝트가 있을 경우
                    Debug.Log($"새로운 GrabbedObject: {grabbedObject.name}");
                }
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

        Vector3 camForward = player.camTransform.forward;
        camForward.y = 0f;
        Vector3 moveDir = player.moveDir;

        // 내적 계산 후 push인지 pull인지 결정
        float dotProduct = Vector3.Dot(camForward, moveDir);
        //Debug.Log($"벡터 내적 : {dotProduct}");

        if (dotProduct > 0f)
        {
            // 밀기
            Debug.Log("밀기");
            grabbedObjectRb.velocity = moveDir * player.model.grabForce;
            // 밀기 애니메이션 재생
            // 이미 재생중이라면 애니메이션 중복 재생x 밀기 당기기 바꿀때만 재생

            // 기존
            //if(!player.view.GetBoolInGrabAnimation(1))
            //{
            //    player.view.SetBoolInGrabAnimation(0, false);
            //    player.view.SetBoolInGrabAnimation(1, true);
                
            //}
            if(!player.view.GetBoolParameter(E_AniParameters.Pushing))
            {
                player.view.SetBoolParameter(E_AniParameters.Pushing, true);
                player.view.SetBoolParameter(E_AniParameters.Pulling, false);
            }

            //grabbedObjectRb.AddForce(moveDir * player.model.grabForce, ForceMode.Force);
        }
        else if(dotProduct < 0f)
        {
            // 당기기
            Debug.Log("당기기");
            grabbedObjectRb.velocity = moveDir * player.model.grabForce;
            //if (!player.view.GetBoolInGrabAnimation(0)) //
            //{
            //    player.view.SetBoolInGrabAnimation(1, false); // pull 종료
            //    player.view.SetBoolInGrabAnimation(0, true); // push 시작
            //}
            if(!player.view.GetBoolParameter(E_AniParameters.Pulling))
            {
                player.view.SetBoolParameter(E_AniParameters.Pushing, false);
                player.view.SetBoolParameter(E_AniParameters.Pulling, true);
            }
            

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
