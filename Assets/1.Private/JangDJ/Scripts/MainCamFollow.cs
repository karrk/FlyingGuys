using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamFollow : MonoBehaviour
{
    private CinemachineBrain _brain;

    private void Start()
    {
        _brain = GetComponent<CinemachineBrain>();
    }

    private void LateUpdate()
    {
        if (_brain.ActiveVirtualCamera == null)
            return;

        this.transform.position = _brain.ActiveVirtualCamera.VirtualCameraGameObject.transform.transform.position;
    }
}
