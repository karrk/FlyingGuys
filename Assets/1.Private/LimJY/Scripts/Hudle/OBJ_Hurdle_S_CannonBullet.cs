using Photon.Pun;
using System;
using UnityEngine;

public class OBJ_Hurdle_S_CannonBullet : MonoBehaviourPun, IPooledObject, IPunObservable
{
    public float returnTime;
    private float remainTime;
    private Rigidbody rb;

    public Enum MyType => E_Object.Cannon_Small;
    public GameObject MyObject => gameObject;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        remainTime = returnTime;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    private void Update()
    {
        remainTime -= Time.deltaTime;

        if (remainTime < 0)
        {
            Return();
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(this);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
            stream.SendNext(rb.rotation);
        }
        else if (stream.IsReading)
        {
            rb.position = (Vector3)stream.ReceiveNext();
            rb.velocity = (Vector3)stream.ReceiveNext();
            rb.rotation = (Quaternion)stream.ReceiveNext();
        }

        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
        rb.position += rb.velocity * lag;
    }
}
