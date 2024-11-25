using System;
using UnityEngine;

public abstract class CustomParticleSystem : MonoBehaviour, IPooledObject
{
    [SerializeField] public E_VFX VFXType;
    public Enum MyType => VFXType;

    public GameObject MyObject => gameObject;

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(this);
    }

    private ParticleSystem _particle;

    private void Start()
    {
        _particle = GetComponent<ParticleSystem>();
        InitOptions();
    }

    protected virtual void InitOptions()
    {
        var main = _particle.main;
        main.playOnAwake = false;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    // 루핑으로 설정된 파티클은 동작하지 않음
    protected virtual void OnParticleSystemStopped()
    {
        Return();
    }
}