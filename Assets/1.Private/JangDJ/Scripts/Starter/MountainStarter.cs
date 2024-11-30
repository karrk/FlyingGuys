using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MountainStarter : StageStarter
{
    private PlayableDirector _timeline;
    [SerializeField] private CinemachineVirtualCamera _dollyCam;
    private CinemachineTrackedDolly _dolly;
    private CinemachineComposer _composer;

    private void Start()
    {
        _timeline = GetComponent<PlayableDirector>();
        
        _dolly = _dollyCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        _composer = _dollyCam.GetCinemachineComponent<CinemachineComposer>();

        _dolly.m_AutoDolly.m_PositionOffset = 0;
        _composer.m_ScreenY = 0.5f;
    }

    public void EndTimeLine()
    {
        StartCoroutine(MountainStart());
    }

    private IEnumerator MountainStart()
    {
        yield return StageFXs.Instance.StartCountDown();
        NetWorkManager.IsPlay = true;
        StartCoroutine(StageFXs.Instance.StepPlayFX());
    }

    public override void StageStart()
    {
        _timeline.Play();
    }
}
