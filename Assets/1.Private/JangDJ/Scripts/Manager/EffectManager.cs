using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public void PlayFX(Vector3 requestPos, E_VFX vfxType)
    {
        ParticleSystem particle = ObjPoolManager.Instance.GetObject<ParticleSystem>(vfxType);

        particle.transform.position = requestPos;
        particle.Play();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PlayFX(Vector3.up * 1, E_VFX.Grab);
    }
}
