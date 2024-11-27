using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InsertControll : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;

    public void JoinInGame()
    {
        if (nameInput.text == "")
        {
            Debug.LogWarning("랜덤한 닉네임으로 지정합니다.");
            NetWorkManager.NickName = $"Player {Random.Range(100, 1000)}";
        }
        else
        {
            NetWorkManager.NickName = nameInput.text;
        }

        SceneManager.LoadScene("UI_MainMenu");
    }

    // TODO : 로그인 관련 코드 작성
}
