using Photon.Pun;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameEndPopUp : MonoBehaviourPun
{
    [SerializeField] Image GameEndImage;
    [SerializeField] GameObject GameEndText; // 추후 이미지로 수정 예정

    [SerializeField] GameObject CrownOBJ;

    private PhotonView photonBiew;
    private Coroutine Coroutine;


    private void Awake()
    {
        GameEndImage.fillAmount = 0.0f;
        GameEndText.SetActive(false);
    }

    public void GameEndPopUp()
    {
        // 왕관 오브젝트가 Scene에 없을 때,
        if (CrownOBJ == null)
        {
           StartCoroutine(EndPopUpRoutine());
        }

        // TODO : 결과 씬으로 이동
    }

    IEnumerator EndPopUpRoutine()
    {
        while (GameEndImage.fillAmount < 1.0f)
        {
            Debug.Log("실행됨!");
            GameEndImage.fillAmount += 2f * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        GameEndText.SetActive(true);
        yield return null;
    }
}
