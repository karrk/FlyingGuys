using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGroundStarter : MonoBehaviour, IStarter
{
    [SerializeField] private GameObject _emptyBoard;
    [SerializeField] private Animation _uiAnim;

    public void StartStage()
    {
        StartCoroutine(FallStart());
    }

    public IEnumerator FallStart()
    {
        _uiAnim.Play();
        yield return StageFXs.Instance.StartCountDown();
        NetWorkManager.IsPlay = true;
        StartCoroutine(StageFXs.Instance.StepPlayFX());
        _emptyBoard.SetActive(false);
    }
}
