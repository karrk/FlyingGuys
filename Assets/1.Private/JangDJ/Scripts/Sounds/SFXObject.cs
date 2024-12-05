using System;
using System.Collections;
using UnityEngine;

public class SFXObject : MonoBehaviour, IPooledObject
{
    public Enum MyType => E_Object.SoundSource;

    public GameObject MyObject => gameObject;

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(this);
    }

    [SerializeField] private float _clipLength;
    [SerializeField] private AudioSource _source;
    private bool _isPlaying = false;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void Init(AudioClip clip, Vector3 position)
    {
        this.transform.position = position;
        _isPlaying = true;
        this._source.clip = clip;
        _clipLength = clip.length;
    }

    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        _source.Play();
        yield return new WaitForSeconds(_clipLength);
        Return();
    }

    private void OnDisable()
    {
        if (_isPlaying == true)
            StopAllCoroutines();

        _isPlaying = false;
    }
}
