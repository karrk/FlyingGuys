using Photon.Pun;
using UnityEngine;

public class MenuColorLoader : MonoBehaviour
{
    [SerializeField] public Renderer Renderer;
    private void OnEnable()
    {
        SetColor(PhotonNetwork.LocalPlayer.GetColor());
    }
    public void SetColor(Color color)
    {
        Renderer.material.color = color;
    }
    public void SetColor(Vector3 colorValue)
    {
        Color color = new Color(colorValue.x, colorValue.y, colorValue.z);
        Renderer.material.color = color;
    }
}