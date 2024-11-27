using Photon.Pun;
using System.Collections.Specialized;
using UnityEngine;

public class RPCDelegate : MonoBehaviourPun
{
    public static RPCDelegate Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    public void PlayFX(Vector3 requestPos, E_VFX vfxType)
    {
        photonView.RPC(nameof(PlayFXRPC), RpcTarget.All, requestPos, vfxType);
    }

    [PunRPC]
    private void PlayFXRPC(Vector3 requestPos, E_VFX vfxType)
    {
        EffectManager.Instance.PlayFX(requestPos, vfxType, E_NetworkType.Private);
    }

    public void DeadPlayer(int viewId)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;

        photonView.RPC(nameof(DeadPlayerRPC), RpcTarget.All, viewId);
    }

    [PunRPC]
    private void DeadPlayerRPC(int viewId)
    {
        PhotonNetwork.Destroy(PhotonNetwork.GetPhotonView(viewId).gameObject);
    }

    public void DestroyDropFloor(Vector3 targetPos)
    {
        photonView.RPC(nameof(DestroyDropFloorRPC), RpcTarget.All, targetPos);
    }

    [PunRPC]
    private void DestroyDropFloorRPC(Vector3 targetpos)
    {
        float upOffset = 0.5f;
        int groundLayer = 1 << 6;
        RaycastHit hit;

        if(Physics.Raycast(targetpos + Vector3.up * upOffset,
            Vector3.down, out hit, 1f,groundLayer))
        {
            Destroy(hit.collider.gameObject);
        }

        //Debug.DrawRay(targetpos + Vector3.up * upOffset,
        //    Vector3.down * 1f, Color.red, 5f);
    }

    public void PlayStartFX(StageFXs component)
    {
        photonView.RPC(nameof(PlayStartFXRPC), RpcTarget.All);
    }

    [PunRPC]
    private void PlayStartFXRPC()
    {
        
    }
}
