using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamController : MonoBehaviour
{
    private CinemachineBrain _brain;
    public int ID { get; private set; }
    private int _decreaseValue = 0;

    private void Start()
    {
        _brain = GetComponent<CinemachineBrain>();
    }

    /// <summary>
    /// 로컬 플레이어의 view ID를 등록합니다.
    /// </summary>
    public void SetID(int id)
    {
        this.ID = id;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _decreaseValue -= 1;

            if (ID != _brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CamController>().ID)
                this._brain.ActiveVirtualCamera.Priority = _decreaseValue;
        }
    }

    private void LateUpdate()
    {
        FollowVirtualCam();
    }

    private void FollowVirtualCam()
    {
        if (_brain.ActiveVirtualCamera == null)
            return;

        this.transform.position = _brain.ActiveVirtualCamera.VirtualCameraGameObject.transform.transform.position;
    }
}
