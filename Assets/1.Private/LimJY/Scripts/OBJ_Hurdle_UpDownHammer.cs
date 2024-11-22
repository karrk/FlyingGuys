using UnityEngine;

public class OBJ_Hurdle_UpDownHammer : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("위아래 망치 강타!");
        }
    }
}
