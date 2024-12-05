using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class UI_ResetPassPanel : MonoBehaviour
{
    [SerializeField] GameObject Confirm;
    [SerializeField] GameObject Re;

    [Space(10f)]
    [SerializeField] ErrorText ErrorText;
    [SerializeField] TMP_InputField NickInputField;
    [SerializeField] TMP_InputField EmailInputField;

    private DatabaseReference user;


    private void OnEnable()
    {
        user = BackendManager.DataBase.RootReference.Child("Users");
        Confirm.SetActive(true);
        Re.SetActive(false);

        EmailInputField.text = "";
        NickInputField.text = "";
    }

    public async void DataConfirm()
    {
        string UserNick = BackendManager.Auth.CurrentUser.DisplayName;
        string UserID = BackendManager.Auth.CurrentUser.UserId;
        DatabaseReference user = BackendManager.DataBase.RootReference.Child("Users");

        string Nick = NickInputField.text;
        string email = EmailInputField.text;


        await user.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snapshot = task.Result;

            if (snapshot.HasChild(UserID) == true)
            {
                string myNick = (string)snapshot.Child(UserID).Child("nickname").Value;
                string myEmail = (string)snapshot.Child(UserID).Child("email").Value;


                if (Nick == myNick && email == myEmail)
                {
                    Confirm.SetActive(false);
                    Re.SetActive(true);
                    BackendManager.Auth.SendPasswordResetEmailAsync(email);
                }
                else
                {
                    ErrorText.ChangeText("닉네임 또는 이메일이 일치하지 않습니다.");
                    return;
                }
            }
        });
    }
}
