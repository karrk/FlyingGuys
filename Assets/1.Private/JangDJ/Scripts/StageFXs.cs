using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFXs : MonoBehaviour
{
    public void PlayChildFXs()
    {
        foreach (var particle in GetComponentsInChildren<ParticleSystem>())
        {
            particle.Play();
        }
    }
}
