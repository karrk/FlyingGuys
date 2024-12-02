using UnityEngine;

public class UI_GameExitButton : MonoBehaviour
{
    public void GameExit()
    {
        BackendManager.Instance.Logout();
    }
}
