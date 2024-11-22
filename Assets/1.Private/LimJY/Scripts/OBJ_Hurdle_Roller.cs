using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_Hurdle_Roller : MonoBehaviour
{
    [SerializeField] private Vector3 rotate;

    private void FixedUpdate()
    {
        transform.Rotate(rotate);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("회전 장애물 피격당함!");
        }
    }
}
