using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_Hurdle_Conveyor : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 dir;
    [SerializeField] List<GameObject> onBelt; // 벨트에 닿은 오브젝트를 저장하는 리스트

    [Header("velt move speed")]
    [SerializeField] Vector2 Scroll;


    private void OnCollisionEnter(Collision collision)
    {
        // 벨트에 어떠한 오브젝트가 닿았을 때, 해당 오브젝트를 리스트에 보관한다.
        onBelt.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        // 벨트에 닿았언 어떠한 오브젝트가 떠났을 때, 해당 오브젝트를 리스트에서 제외한다.
        onBelt.Remove(collision.gameObject);
    }

    // ==== === ====

    private void Update()
    {
        // 컨베이어 벨트 오브젝트의 속도 조절
        Vector2 Offset = new Vector2 ((Time.time * Scroll.x), -(Time.time * Scroll.y));
        GetComponent<Renderer>().material.mainTextureOffset = Offset;

        // 리스트에 저장된 오브젝트들의 rigidbody를 참조하여, 해당 값 직접적으로 증가 시킨다.
        foreach (GameObject obj in onBelt)
        {
            obj.GetComponent<Rigidbody>().velocity = moveSpeed * dir;
        }
    }
}
