using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGroundStarter : StageStarter
{
    [SerializeField] private GameObject _emptyBoard;
    [SerializeField] private Animation _uiAnim;

    public IEnumerator FallStart()
    {
        _uiAnim.Play();
        yield return StageFXs.Instance.StartCountDown();
        NetWorkManager.IsPlay = true;
        StartCoroutine(StageFXs.Instance.StepPlayFX());
        _emptyBoard.SetActive(false);
    }

    public override void StageStart()
    {
        StartCoroutine(FallStart());
    }
}
