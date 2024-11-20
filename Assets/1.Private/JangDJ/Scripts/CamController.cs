using Cinemachine;
using UnityEngine;

public class CamController : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    private CinemachineOrbitalTransposer _tr;
    private CinemachineComposer _aim;

    [Header("Cam Y Clamp")]
    [SerializeField, Range(-10f, 10f)] private float _minRotY;
    [SerializeField, Range(10f, 60f)] private float _maxRotY;

    [Header("Cam Z Clamp")]
    [SerializeField, Range(10, 5)] private float _minTrZ;
    [SerializeField, Range(20, 5)] private float _maxTrZ;

    private float _clampValue;
    private float _normalizeValue;
    private float _resultZ;

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _tr = _cam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _aim = _cam.GetCinemachineComponent<CinemachineComposer>();
    }

    /// <summary>
    /// value 값으로 카메라 상하 이동, x축을 회전합니다.
    /// </summary>
    public void RotY(float value)
    {
        _clampValue = Mathf.Clamp(_tr.m_FollowOffset.y + value, _minRotY, _maxRotY);
        _normalizeValue = (_clampValue - _minRotY) / (_minRotY - _maxRotY);
        _resultZ = Mathf.Lerp(-_maxTrZ, -_minTrZ, -_normalizeValue);

        _tr.m_FollowOffset = Vector3.up * _clampValue + Vector3.forward * _resultZ;
    }

    /// <summary>
    /// 카메라의 우선순위 값을 덮어 씌웁니다.
    /// </summary>
    public void SetForcePriority(int value)
    {
        _cam.Priority = value;
    }

    /// <summary>
    /// 해당 카메라의 우선순위를 1 내립니다.
    /// </summary>
    public void MinusOnePriority()
    {
        _cam.Priority--;
    }
}
