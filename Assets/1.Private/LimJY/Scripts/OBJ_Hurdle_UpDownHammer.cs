using UnityEngine;

public class OBJ_Hurdle_UpDownHammer : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    private Vector3 Updown;


    private void Update()
    {
        if (gameObject.transform.rotation.x < 0f)
        {
            Updown = Vector3.down;
        }
        else if (gameObject.transform.rotation.x > 180f)
        {
            Updown = Vector3.up;
        }

        transform.Rotate(Updown, rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("위아래 망치 강타!");
        }
    }
}
