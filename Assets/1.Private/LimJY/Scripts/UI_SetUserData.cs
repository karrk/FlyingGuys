using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class UI_SetUserData : MonoBehaviour
{
    [SerializeField] GameObject SttingPanel;
    [SerializeField] GameObject _ConfirmPanel;
    [SerializeField] GameObject _SetPanel;

    [Header("Confirm")]
    [SerializeField] ErrorText ConfirmErrorText;
    [SerializeField] TMP_InputField C_EmailInputField;
    [SerializeField] TMP_InputField C_PassInputField;

    [Header("Set")]
    [SerializeField] ErrorText SetErrorText;
    [SerializeField] TMP_InputField Re_NickInputField;
    [SerializeField] TMP_InputField Re_EmailInputField;
    [SerializeField] TMP_InputField Re_PassInputField;

    FirebaseUser curUser;
    private DatabaseReference user;

    private void OnEnable()
    {
        curUser = BackendManager.Auth.CurrentUser;
        user = BackendManager.DataBase.RootReference.Child("Users");

        _ConfirmPanel.SetActive(true);
        _SetPanel.SetActive(false);

        C_EmailInputField.text = "";
        C_PassInputField.text = "";

        Re_NickInputField.text = "";
        Re_EmailInputField.text = "";
        Re_PassInputField.text = "";
    }

    public async void DataConfirm()
    {
        string userID = curUser.UserId;
        string email = C_EmailInputField.text;
        string pass = C_PassInputField.text;


        await user.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.HasChild(userID) == true)
                {
                    string myEmail = (string)snapshot.Child(userID).Child("email").Value;

                    if (email != myEmail)
                    {
                        ConfirmErrorText.ChangeText("이메일이 잘못되었습니다. 다시 입력해주세요.");
                    }
                    else
                    {
                        _ConfirmPanel.SetActive(false);
                        _SetPanel.SetActive(true);
                    }
                }
            });
    }

    public async void DataSet()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        string userID = curUser.UserId;

        string nick = Re_NickInputField.text;
        string email = Re_EmailInputField.text;
        string pass = Re_PassInputField.text;


        await user.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snap = task.Result;

            if (snap.HasChild(userID) == true)
            {
                string curNick = (string)snap.Child(userID).Child("nickname").Key;
                string curEmail = (string)snap.Child(userID).Child("email").Key;


                if (nick.IsNullOrEmpty()) { }
                else if (nick == (string)snap.Child(curNick).Value)
                {
                    SetErrorText.ChangeText("이미 사용중인 정보입니다.");
                    return;
                }
                else dic[snap.Child(curNick).Key] = nick;


                if (email.IsNullOrEmpty()) { }
                else if (email == (string)snap.Child(curEmail).Value)
                {
                    SetErrorText.ChangeText("이미 사용중인 정보입니다.");
                    return;
                }
                else
                {
                    dic[snap.Child(curEmail).Key] = email;
                    curUser.SendEmailVerificationBeforeUpdatingEmailAsync(email);
                }


                if (pass.IsNullOrEmpty()) { }
                else curUser.UpdatePasswordAsync(pass);



                user.Child(userID).UpdateChildrenAsync(dic).ContinueWithOnMainThread(task =>
                {
                    SttingPanel.SetActive(true);
                    this.gameObject.SetActive(false);
                });
            }
        });


    }
}
