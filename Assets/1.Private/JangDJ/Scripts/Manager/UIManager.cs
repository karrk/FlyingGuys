using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IManager
{
    public static UIManager Instance { get; private set; }
    [SerializeField] Button[] Buttons;

    private Coroutine cor;

    public void Init()
    {
        Instance = this;
    }


    void Awake()
    {
        SceneManager.sceneLoaded += ButtonSFX;
    }

    void LateUpdate()
    {
        StartCoroutine(BGMPlay());
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

    // ㅠㅠ
    IEnumerator BGMPlay()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Public_Login": SoundManager.Instance.Play(E_BGM.BG_Login, 0f, 0f); yield break;
            case "Public_Menu": SoundManager.Instance.Play(E_BGM.BG_MainMenu, 0f, 0f); yield break;
            case "Public_Loading": SoundManager.Instance.Play(E_BGM.BG_Loading, 0f, 0f); yield break;
            case "Public_Result": SoundManager.Instance.Play(E_BGM.BG_GameResult, 0f, 0f); yield break;
            default: yield break;
        }
    }
}
