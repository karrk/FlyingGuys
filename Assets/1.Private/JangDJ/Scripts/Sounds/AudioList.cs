using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioList<T> : MonoBehaviour where T : Enum
{
    [SerializeField] protected List<ClipContent<T>> _list;
    public AudioSource Source => GetComponent<AudioSource>();

    public AudioClip this[T type]
    {
        get { return GetClip(type); }
    }

    private AudioClip GetClip(T type)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (type.ToString() == _list[i].Name.ToString())
                return _list[i].Clip;
        }

        Debug.LogError($"{type} 음원을 찾을 수 없습니다.");
        return null;
    }
}

[System.Serializable]
public class ClipContent<T> where T : Enum
{
    public T Name;
    public AudioClip Clip;
}
