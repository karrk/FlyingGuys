using Photon.Pun;
using UnityEngine;

public class EffectManager : MonoBehaviour, IManager
{
    public static EffectManager Instance { get; private set; }
    [SerializeField] public RPCDelegate Del;

    public void Init()
    {
        Instance = this;
    }

    // 개인만 생성해야하는 오브젝트가 있고
    // 전체가 생성해야하는 오브젝트가 있음
    public void PlayFX(Vector3 requestPos, E_VFX vfxType, E_NetworkType networkType)
    {
        if(networkType == E_NetworkType.Public)
        {
            Del.PlayFX(requestPos, vfxType);
            return;
        }

        ParticleSystem particle = ObjPoolManager.Instance.GetObject<ParticleSystem>(vfxType);

        particle.transform.position = requestPos;
        particle.Play();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PlayFX(Vector3.up * 1, E_VFX.Grab,E_NetworkType.Public);
    }
}
