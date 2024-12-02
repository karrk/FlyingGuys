using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private bool _isRecoveryMode;
    [SerializeField] private Transform[] _recoverPoints;
    public Test_GameScene _myScene;
    public Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isRecoveryMode)
                other.transform.position
                    = FindClosetPoint(other.transform.position);
            else
            {
                if (other.TryGetComponent<PhotonView>(out PhotonView view))
                {
                    int num = other.GetComponent<PlayerController>().model.playerNumber;
                    _myScene.DeadPlayer(num);

                    DeadLogic(view.ViewID);
                    player = (Player)view.InstantiationData[0];
                    player.SetLife(false);
                }
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

    private void DeadLogic(int characterViewID)
    {
        RPCDelegate.Instance.DeadPlayer(characterViewID);
    }
}
