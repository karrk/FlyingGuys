using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Audio_Speaker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] List<Button> buttons;

    public PointerEventData data;

    private void LateUpdate()
    {
        //if (buttons[].animationTriggers.highlightedTrigger) E_Button_Highlighted();

        foreach (Button button in buttons)
        {
            button.OnPointerEnter(data);
            button.OnPointerClick(data);
            button.OnPointerExit(data);

        }
    }

    public void OnPointerEnter(PointerEventData eventData) => SoundManager.Instance.Play(E_UISFX.S_Highlighted);
    public void OnPointerClick(PointerEventData eventData) => SoundManager.Instance.Play(E_UISFX.S_Pressed);
    public void OnPointerExit(PointerEventData eventData) => SoundManager.Instance.Play(E_UISFX.S_Exit);


    void E_Button_GamePlay() => SoundManager.Instance.Play(E_UISFX.S_GamePlay);
    void E_Button_LoadingArrowLeft() => SoundManager.Instance.Play(E_UISFX.S_LoadingArrowLeft);
    void E_Button_LoadingArrowRight() => SoundManager.Instance.Play(E_UISFX.S_LoadingArrowRight);
    void E_Button_Winner() => SoundManager.Instance.Play(E_UISFX.S_Winner);
    void E_Button_Loser() => SoundManager.Instance.Play(E_UISFX.S_Loser);
}
