using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_Hurdle_CannonBullet : MonoBehaviour
{
    public  float returnTime;
    private float remainTime;


    private void OnEnable()
    {
        remainTime = returnTime;
    }

    private void Update()
    {
        remainTime -= Time.deltaTime;

        if (remainTime < 0)
        {
            // TODO : 오브젝트 풀 패턴 적용 예정
        }
    }
}
