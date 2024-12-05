using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltController : MonoBehaviour
{
    public float speed;
    //public float maxSpeed;
    public Vector3 direction;
    public Vector3 conveyorVelocity;
    public List<GameObject> onBelt;

    private void Awake()
    {
        // 임시 조정값들
        speed = 5.0f;
        direction = new Vector3(0, 0, -1); 

    }
    void Start()
    {
        conveyorVelocity = direction * speed;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        //for (int i = 0; i <= onBelt.Count - 1; i++)
        //{
        //    Rigidbody targetRb =  onBelt[i].GetComponent<Rigidbody>();
        //    if(targetRb != null)
        //    {
        //        //Debug.Log("컨베이어 벨트 위에 물체가 있음");
                
        //        targetRb.velocity += conveyorVelocity;
        //        Debug.Log(targetRb.velocity.sqrMagnitude);
        //        if (targetRb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        //        {
        //            Debug.Log("최대속도 조절중");
        //            targetRb.velocity = targetRb.velocity.normalized * maxSpeed;
        //        }


        //        ////targetRb.velocity = direction * speed;
        //    }




        //    //targetRb.velocity += speed * direction * Time.fixedDeltaTime;+
        //    //targetRb.AddForce(direction * speed, ForceMode.Force);
        //    //if(targetRb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        //    //{
        //    //    Debug.Log("컨베이어 벨트 위 물체 이동속도 제한됨");
        //    //    targetRb.velocity = targetRb.velocity * maxSpeed;
        //    //}
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        onBelt.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        onBelt.Remove(collision.gameObject);
    }
}
