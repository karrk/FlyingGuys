using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IManager
{
    public static UIManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;
    }
}
