using Unity.VisualScripting;
using UnityEngine;

public class OBJ_Hurdle_Hammer : MonoBehaviour
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
            Debug.Log("망치 피격당함!");
        }        
    }
}
