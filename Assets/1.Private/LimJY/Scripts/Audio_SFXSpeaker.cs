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
            default: SoundManager.Instance.Play(E_SFX.S_Highlighted); break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "_GamePlayButton": SoundManager.Instance.Play(E_SFX.S_GamePlay); break;
            case "leftButton": SoundManager.Instance.Play(E_SFX.S_LoadingArrowLeft); break;
            case "rightButton": SoundManager.Instance.Play(E_SFX.S_LoadingArrowRight); break;
            default: SoundManager.Instance.Play(E_SFX.S_Pressed); break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "leftButton": break;
            case "rightButton": break;
            default: SoundManager.Instance.Play(E_SFX.S_Exit); break;
        }
    }
}
