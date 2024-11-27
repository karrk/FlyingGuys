using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    public void OnGrabbedEnter()
    {
        Debug.Log($"나 {gameObject.name} 잡혔다 !");
    }

    public void OnGrabbedLeave()
    {
        Debug.Log($"나 {gameObject.name} 풀려났다 !");
    }
}
