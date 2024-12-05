//using Photon.Pun;
//using System.Collections;
//using UnityEngine;

//public class OBJ_Hurdle_CannonSmall : MonoBehaviourPunCallbacks
//{
//    [SerializeField] Transform muzzlePoint;

//    [Header("Field")]
//    [SerializeField] private bool IsGamePlaying;
//    [SerializeField] float ShotCool;
//    [SerializeField] float ShootSpeed;

//    [Header("Bullet")]
//    [SerializeField] float BulletRetrunTime;


//    void Start()
//    {
//        IsGamePlaying = true;
//        StartCoroutine(IShot(ShotCool));
//    }

//    IEnumerator IShot(float wait)
//    {
//        yield return new WaitForSeconds(5f);

//        while (IsGamePlaying)
//        {
//            if (!PhotonNetwork.LocalPlayer.IsMasterClient) yield break;

//            Debug.Log("IShot 코루틴 실행됨");

//            photonView.RPC(nameof(Shot), RpcTarget.All, muzzlePoint.position, muzzlePoint.rotation);
//            yield return new WaitForSeconds(wait);
//        }
//    }

//    [PunRPC]
//    public void Shot(Vector3 position, Quaternion rotation)
//    {
//        GameObject bulletOBJ = ObjPoolManager.Instance.GetObject(E_Object.Cannon);
//        bulletOBJ.GetComponent<Rigidbody>().position = position;
//        bulletOBJ.GetComponent<Rigidbody>().rotation = rotation;

//        bulletOBJ.GetComponent<OBJ_Hurdle_CannonBullet>().returnTime = BulletRetrunTime;
//        bulletOBJ.GetComponent<Rigidbody>().velocity = ShootSpeed * muzzlePoint.forward;
//    }


//}
