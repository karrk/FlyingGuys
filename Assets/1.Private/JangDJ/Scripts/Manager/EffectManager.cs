using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public void PlayFX(Vector3 requestPos)
    {
        ParticleSystem particle = ObjPoolManager.Instance.GetObject<ParticleSystem>(E_VFX.Grab);

        particle.transform.position = requestPos;
        particle.Play();
    }
}
