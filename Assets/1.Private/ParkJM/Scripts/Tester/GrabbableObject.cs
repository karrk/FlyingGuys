using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    private bool isAlreadyGrabbed;

    public void OnGrabbedEnter()
    {
        if (isAlreadyGrabbed)
            return;

        Debug.Log($"나 {gameObject.name} 잡혔다 !");
        isAlreadyGrabbed = true;
    }

    public void OnGrabbedLeave()
    {
        Debug.Log($"나 {gameObject.name} 풀려났다 !");
        isAlreadyGrabbed = false;
    }
}
