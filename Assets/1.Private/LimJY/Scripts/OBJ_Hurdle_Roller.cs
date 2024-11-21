using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_Hurdle_Roller : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.right, rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("회전 망치 강타!");
        }
    }
}
