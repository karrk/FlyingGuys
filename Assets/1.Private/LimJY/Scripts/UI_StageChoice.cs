using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageChoice : MonoBehaviour
{
    [SerializeField] private PlayMatch playMath;
    [SerializeField] List<Sprite> MapScreenShot;

    [Header("변경내용")]
    [SerializeField] Image CurScreenShot;
    [SerializeField] TMP_Text CossName;
    [SerializeField] TMP_Text CossEx;


    private void Start()
    {
        playMath = GetComponent<PlayMatch>();

        CurScreenShot.sprite = null;
        CossName.text = "";
        CossEx.text = "";

        playMath.onChangeStageNum += OnStageSet;
    }

    void OnStageSet()
    {
        Debug.Log("스테이지 선정 완료");

        switch (playMath.num)
        {
            case 0: // 산 무너져유
                Mountine();
                break;
            case 1: // 바닥 떨어져유
                Floor();
                break;
            case 2: // 점프 쇼다운
                Jump();
                break;
            case 3: // 롤아웃
                Roll();
                break;
        }
    }

    // === === === 

    void Mountine()
    {
        CurScreenShot.sprite = MapScreenShot[playMath.num];
        CossName.text = "산 무너져유";
        CossEx.text = "산이 무너집니다";
    }

    void Floor()
    {
        CurScreenShot.sprite = MapScreenShot[playMath.num];
        CossName.text = "바닥 떨어져유";
        CossEx.text = "바닥이 떨어집니다";
    }

    void Jump()
    {
        CurScreenShot.sprite = MapScreenShot[playMath.num];
        CossName.text = "점프 쇼다운";
        CossEx.text = "나태지옥 : 멈추면 떨어집니다";
    }

    void Roll()
    {
        CurScreenShot.sprite = MapScreenShot[playMath.num];
        CossName.text = "롤 아웃";
        CossEx.text = "무슨 맵이더라";
    }
}
