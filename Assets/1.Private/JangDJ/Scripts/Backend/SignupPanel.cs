using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class SignupPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _pwd;
    [SerializeField] private TMP_InputField _confirm;
    [SerializeField] private ErrorText _error;

    private void OnEnable()
    {
        _email.onEndEdit.AddListener(CheckEmailpattern);
        _pwd.onEndEdit.AddListener(Checkpwds);
    }

    public void SignUp()
    {
        BackendManager.Auth
            .CreateUserWithEmailAndPasswordAsync(_email.text, _pwd.text)
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");
        });
    }

    private void CheckEmailpattern(string text)
    {
        if (text.IsNullOrEmpty() == true)
            return;

        if (!text.Contains('.') || !text.Contains('@') || text[text.Length]-1 == '.')
        {
            _email.text = "";
            _error.ChangeText("옳바른 이메일을 입력해주세요");
        }
    }

    private void Checkpwds(string text)
    {

    }

    private void OnDisable()
    {
        _email.onEndEdit.RemoveAllListeners();
    }
}
