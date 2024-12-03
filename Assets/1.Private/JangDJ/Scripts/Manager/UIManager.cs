using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IManager
{
    public static UIManager Instance { get; private set; }
    [SerializeField] Button[] Buttons;


    public void Init()
    {
        Instance = this;
    }
    void Awake()
    {
        SceneManager.sceneLoaded += ButtonSFX;
        SceneManager.sceneLoaded += BGMPlay;
    }

    void ButtonSFX(Scene s, LoadSceneMode m)
    {
        Array.Clear(Buttons, 0, Buttons.Length);
        Buttons = FindObjectsOfType<Button>();

        foreach (Button button in Buttons)
        {
            button.AddComponent<Audio_SFXSpeaker>();
        }
    }


    void BGMPlay(Scene s, LoadSceneMode m)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Public_Login": SoundManager.Instance.Play(E_BGM.BG_Login, 0f, 0f); return;
            case "Public_Menu": SoundManager.Instance.Play(E_BGM.BG_MainMenu, 0f, 0f); return;
            case "Public_Loading": SoundManager.Instance.Play(E_BGM.BG_Loading, 0f, 0f); return;
            case "Public_Result": SoundManager.Instance.Play(E_BGM.BG_GameResult, 0f, 0f); return;
            default: return;
        }
    }
}
