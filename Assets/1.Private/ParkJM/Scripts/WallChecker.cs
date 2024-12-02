using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public bool IsWallDetected;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"벽 감지됨: {other.name}");
        IsWallDetected = true;

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"벽에서 나감: {other.name}");
        IsWallDetected = false;
    }
}
