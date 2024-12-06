using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class UI_SetUserData : MonoBehaviour
{
    [SerializeField] GameObject SttingPanel;
    [SerializeField] GameObject _ConfirmPanel;
    [SerializeField] GameObject _SetPanel;
    [SerializeField] GameObject _UserDeletePanel;

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
        _UserDeletePanel.SetActive(false);

        C_EmailInputField.text = "";
        C_PassInputField.text = "";

        Re_NickInputField.text = "";
        Re_EmailInputField.text = "";
        Re_PassInputField.text = "";
    }

    public void DataConfirm()
    {
        string email = C_EmailInputField.text;
        string pass = C_PassInputField.text;

        Credential credential = EmailAuthProvider.GetCredential(email, pass);


            curUser.ReauthenticateAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Exception excption = task.Exception.GetBaseException();

                    if (excption is FirebaseException exp)
                    {
                        var errorCode = (AuthError)exp.ErrorCode;

                        switch (errorCode)
                        {
                            case AuthError.InvalidEmail:
                                ConfirmErrorText.ChangeText("잘못된 이메일 입니다");
                                break;
                            case AuthError.WrongPassword:
                                ConfirmErrorText.ChangeText("올바른 비밀번호가 아닙니다");
                                break;
                        }
                    }
                    return;
                }
                    _ConfirmPanel.SetActive(false);
                    _SetPanel.SetActive(true);
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
                    curUser.UpdateEmailAsync(email);
                }


                if (pass.IsNullOrEmpty()) { }
                else curUser.UpdatePasswordAsync(pass);



                user.Child(userID).UpdateChildrenAsync(dic).ContinueWithOnMainThread(task =>
                {
                    StartCoroutine(UI_UserDataPanel.Instance.ChangeNickName());

                    SttingPanel.SetActive(true);
                    this.gameObject.SetActive(false);
                });
            }
        });
    }

    public void DeleteUser()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user != null)
        {
            DatabaseReference userID = BackendManager.DataBase.RootReference.Child("Users").Child(user.UserId);

            userID.RemoveValueAsync();
            user.DeleteAsync();

            PhotonNetwork.LoadLevel("Public_Login");
        }
    }
}
