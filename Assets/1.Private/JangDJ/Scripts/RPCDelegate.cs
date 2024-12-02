using Photon.Pun;
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

    public void PlaySFX(Vector3 requestPos, E_SFX sfxType)
    {
        photonView.RPC(nameof(PlaySFXRPC), RpcTarget.All, requestPos, sfxType);
    }

    [PunRPC]
    private void PlaySFXRPC(Vector3 requestPos, E_SFX sfxType)
    {
        SoundManager.Instance.Play(requestPos, sfxType, E_NetworkType.Private);
    }

    public void DeadPlayer(int viewId)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;

        photonView.RPC(nameof(DeadPlayerRPC), RpcTarget.MasterClient, viewId);
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
    }

    public void PlayStartFX()
    {
        photonView.RPC(nameof(PlayStartFXRPC), RpcTarget.All);
    }

    [PunRPC]
    private void PlayStartFXRPC()
    {
        StartCoroutine(StageFXs.Instance.PlayStartFX());
    }

    public void SetActive(int id, bool value)
    {
        photonView.RPC(nameof(SetActiveRPC), RpcTarget.MasterClient, id, value);
    }

    [PunRPC]
    private void SetActiveRPC(int id, bool value)
    {
        PhotonView.Find(id).gameObject.SetActive(value);
    }
}
