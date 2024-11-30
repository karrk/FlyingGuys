using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPlayer : MonoBehaviour
{
    private static WaitForSeconds wait = new WaitForSeconds(0.1f);
    [SerializeField] private E_VFX[] _vfxTypes;
    private bool _ready = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (_ready == false)
            return;

        if(collision.collider.CompareTag("Player"))
        {
            EffectManager.Instance.PlayFX(collision.contacts[0].point,
                GetFXType(), E_NetworkType.Public);

            StartCoroutine(CoolTime());
        }
    }

    public E_VFX GetFXType()
    {
        return _vfxTypes[Random.Range(0, _vfxTypes.Length)];
    }

    private IEnumerator CoolTime()
    {
        _ready = false;
        yield return wait;
        _ready = true;
    }
}
