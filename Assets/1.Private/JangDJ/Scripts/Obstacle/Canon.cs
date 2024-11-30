using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public Vector3 MuzzlePos { get; private set; }
    public Animation Anim { get; private set; }

    private void Start()
    {
        MuzzlePos = transform.GetChild(0).position;
        Anim = GetComponent<Animation>();
    }
}
