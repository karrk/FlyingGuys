using UnityEngine;

public class SoundManager : MonoBehaviour, IManager
{
    public static SoundManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;
    }
}
