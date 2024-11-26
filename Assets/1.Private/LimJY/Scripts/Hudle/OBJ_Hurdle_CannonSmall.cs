using System.Collections;
using UnityEngine;

public class OBJ_Hurdle_CannonSmall : MonoBehaviour
{
    [SerializeField] Transform muzzlePoint;
    [SerializeField] GameObject bullet;

    [Header("Field")]
    [SerializeField] private bool IsGamePlaying;
    [SerializeField] float ShotCool;
    [SerializeField] float ShootSpeed;

    [Header("Bullet")]
    [SerializeField] float BulletRetrunTime;


    private void Start()
    {
        IsGamePlaying = true;
        StartCoroutine(IShotWait(ShotCool));
    }

    IEnumerator IShotWait(float wait)
    {
        while (IsGamePlaying)
        {
            Rigidbody MyBulletRD = Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation).GetComponent<Rigidbody>();
            MyBulletRD.velocity = ShootSpeed * muzzlePoint.forward;
            MyBulletRD.gameObject.GetComponent<OBJ_Hurdle_CannonBullet>().returnTime = BulletRetrunTime;

            yield return new WaitForSeconds(wait);
        }
    }
}
