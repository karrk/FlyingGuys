using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public bool IsWallDetected;

    private void OnTriggerEnter(Collider other)
    {
        IsWallDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsWallDetected = false;
    }
}
