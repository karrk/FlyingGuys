using UnityEngine;
using UnityEngine.EventSystems;

public class Audio_SFXSpeaker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "leftButton":  break;
            case "rightButton": break;
            default: SoundManager.Instance.Play(E_UISFX.S_Highlighted); break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "_GamePlayButton": SoundManager.Instance.Play(E_UISFX.S_GamePlay); break;
            case "leftButton": SoundManager.Instance.Play(E_UISFX.S_LoadingArrowLeft); break;
            case "rightButton": SoundManager.Instance.Play(E_UISFX.S_LoadingArrowRight); break;
            default: SoundManager.Instance.Play(E_UISFX.S_Pressed); break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "leftButton": break;
            case "rightButton": break;
            default: SoundManager.Instance.Play(E_UISFX.S_Exit); break;
        }
    }
}
