using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private bool _isRecoveryMode;
    [SerializeField] private Transform[] _recoverPoints;
    [HideInInspector] public RPCDelegate Del;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(_isRecoveryMode)
                other.transform.position 
                    = FindClosetPoint(other.transform.position);
            else
            {
                DeadLogic(other.gameObject);
            }
        }
    }

    private Vector3 FindClosetPoint(Vector3 curPos)
    {
        int idx = -1;
        float dist = float.MaxValue;
        float tempDist;

        for (int i = 0; i < _recoverPoints.Length; i++)
        {
            tempDist = Vector3.Distance(curPos, _recoverPoints[i].position);

            if (tempDist <= dist)
            {
                idx = i;
                dist = tempDist;
            }
        }

        if (idx == -1)
            Debug.LogError("설정된 체크포인트가 없음");

        return _recoverPoints[idx].position;
    }

    private void DeadLogic(GameObject player)
    {
        // 임시
        Del.DeadPlayer(player);
    }
}
