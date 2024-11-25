using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UIManager : MonoBehaviour, IManager
{
    public static UI_UIManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;
    }
}
