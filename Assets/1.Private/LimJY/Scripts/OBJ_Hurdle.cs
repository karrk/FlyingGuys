using Unity.VisualScripting;
using UnityEngine;

public class OBJ_Hurdle : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("강타!");
        }        
    }
}
