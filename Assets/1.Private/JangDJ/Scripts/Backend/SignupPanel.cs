using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class SignupPanel : MonoBehaviour
{
    [SerializeField] private LoginPanel _loginPanel;
    [SerializeField] private NicknamePanel _nickPanel;

    [Space(10f)]
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _pwd;
    [SerializeField] private TMP_InputField _confirm;
    [SerializeField] private ErrorText _error;

    private bool _isUnmatchPwd = false;

    private void OnEnable()
    {
        _email.onEndEdit.AddListener(CheckEmailpattern);
        _pwd.onEndEdit.AddListener(Checkpwds);
        _confirm.onEndEdit.AddListener(CheckConfirm);
    }

    public void SignUp()
    {
        if (_email.text.IsNullOrEmpty() == true)
        {
            _error.ChangeText("이메일을 입력해주세요");
            return;
        }
        else if (_isUnmatchPwd == true)
        {
            _error.ChangeText("비밀번호가 서로 다릅니다");
            return;
        }

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
                Exception excption = task.Exception.GetBaseException();

                if (excption is FirebaseException exp)
                {
                    var errorCode = (Firebase.Auth.AuthError)exp.ErrorCode;

                    Debug.Log(errorCode);

                    switch (errorCode)
                    {
                        case AuthError.EmailAlreadyInUse:
                            _error.ChangeText("이미 사용중인 계정입니다");
                            _email.text = "";
                            _pwd.text = "";
                            _confirm.text = "";
                            break;
                        #region 에러코드
                        case AuthError.TooManyRequests:
                            break;
                        case AuthError.Failure:
                            break;
                        case AuthError.InvalidCustomToken:
                            break;
                        case AuthError.CustomTokenMismatch:
                            break;
                        case AuthError.InvalidCredential:
                            break;
                        case AuthError.UserDisabled:
                            break;
                        case AuthError.AccountExistsWithDifferentCredentials:
                            break;
                        case AuthError.OperationNotAllowed:
                            break;
                        case AuthError.RequiresRecentLogin:
                            break;
                        case AuthError.CredentialAlreadyInUse:
                            break;
                        case AuthError.InvalidEmail:
                            break;
                        case AuthError.WrongPassword:
                            break;
                        case AuthError.UserNotFound:
                            break;
                        case AuthError.ProviderAlreadyLinked:
                            break;
                        case AuthError.NoSuchProvider:
                            break;
                        case AuthError.InvalidUserToken:
                            break;
                        case AuthError.UserTokenExpired:
                            break;
                        case AuthError.NetworkRequestFailed:
                            break;
                        case AuthError.InvalidApiKey:
                            break;
                        case AuthError.AppNotAuthorized:
                            break;
                        case AuthError.UserMismatch:
                            break;
                        case AuthError.WeakPassword:
                            break;
                        case AuthError.NoSignedInUser:
                            break;
                        case AuthError.ApiNotAvailable:
                            break;
                        case AuthError.ExpiredActionCode:
                            break;
                        case AuthError.InvalidActionCode:
                            break;
                        case AuthError.InvalidMessagePayload:
                            break;
                        case AuthError.InvalidPhoneNumber:
                            break;
                        case AuthError.MissingPhoneNumber:
                            break;
                        case AuthError.InvalidRecipientEmail:
                            break;
                        case AuthError.InvalidSender:
                            break;
                        case AuthError.InvalidVerificationCode:
                            break;
                        case AuthError.InvalidVerificationId:
                            break;
                        case AuthError.MissingVerificationCode:
                            break;
                        case AuthError.MissingVerificationId:
                            break;
                        case AuthError.MissingEmail:
                            break;
                        case AuthError.MissingPassword:
                            break;
                        case AuthError.QuotaExceeded:
                            break;
                        case AuthError.RetryPhoneAuth:
                            break;
                        case AuthError.SessionExpired:
                            break;
                        case AuthError.AppNotVerified:
                            break;
                        case AuthError.AppVerificationFailed:
                            break;
                        case AuthError.CaptchaCheckFailed:
                            break;
                        case AuthError.InvalidAppCredential:
                            break;
                        case AuthError.MissingAppCredential:
                            break;
                        case AuthError.InvalidClientId:
                            break;
                        case AuthError.InvalidContinueUri:
                            break;
                        case AuthError.MissingContinueUri:
                            break;
                        case AuthError.KeychainError:
                            break;
                        case AuthError.MissingAppToken:
                            break;
                        case AuthError.MissingIosBundleId:
                            break;
                        case AuthError.NotificationNotForwarded:
                            break;
                        case AuthError.UnauthorizedDomain:
                            break;
                        case AuthError.WebContextAlreadyPresented:
                            break;
                        case AuthError.WebContextCancelled:
                            break;
                        case AuthError.DynamicLinkNotActivated:
                            break;
                        case AuthError.Cancelled:
                            break;
                        case AuthError.InvalidProviderId:
                            break;
                        case AuthError.WebInternalError:
                            break;
                        case AuthError.WebStorateUnsupported:
                            break;
                        case AuthError.TenantIdMismatch:
                            break;
                        case AuthError.UnsupportedTenantOperation:
                            break;
                        case AuthError.InvalidLinkDomain:
                            break;
                        case AuthError.RejectedCredential:
                            break;
                        case AuthError.PhoneNumberNotFound:
                            break;
                        case AuthError.InvalidTenantId:
                            break;
                        case AuthError.MissingClientIdentifier:
                            break;
                        case AuthError.MissingMultiFactorSession:
                            break;
                        case AuthError.MissingMultiFactorInfo:
                            break;
                        case AuthError.InvalidMultiFactorSession:
                            break;
                        case AuthError.MultiFactorInfoNotFound:
                            break;
                        case AuthError.AdminRestrictedOperation:
                            break;
                        case AuthError.UnverifiedEmail:
                            break;
                        case AuthError.MaximumSecondFactorCountExceeded:
                            break;
                        case AuthError.UnsupportedFirstFactor:
                            break;
                        case AuthError.EmailChangeNeedsVerification:
                            break;
                        default:
                            break;
                            #endregion
                    }
                }

                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");

            string uid = BackendManager.Auth.CurrentUser.UserId;
            DatabaseReference users = BackendManager.DataBase.RootReference.Child("Users");
            bool isUsing = false;

            users.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted == false)
                    return;
                DataSnapshot snapshot = task.Result;


                if (snapshot.HasChild(uid) == false)
                {
                    Debug.Log("신규 가입자");

                    _nickPanel.gameObject.SetActive(true);
                    _nickPanel.RegistedEmail = _email.text;
                    _nickPanel.LoginPanel = _loginPanel;

                    return;
                }
            });

            this.gameObject.SetActive(false);
        });
    }

    private void CheckEmailpattern(string text)
    {
        if (text.IsNullOrEmpty() == true)
            return;

        else if (!text.Contains('.') || !text.Contains('@'))
        {
            _email.text = "";
            _error.ChangeText("올바른 이메일을 입력해주세요");
        }
    }

    private void Checkpwds(string text)
    {
        if (text.IsNullOrEmpty() == true)
            return;

        else if (text.Length <= 6)
        {
            _pwd.text = "";
            _error.ChangeText("비밀번호가 너무 짧습니다");
        }

        else if (_confirm.text.IsNullOrEmpty() == false)
        {
            if (_pwd.text != _confirm.text)
            {
                _error.ChangeText("재확인 비밀번호와 다릅니다", false);
                _isUnmatchPwd = true;
            }
            else
            {
                _isUnmatchPwd = false;
            }
        }
    }

    private void CheckConfirm(string text)
    {
        if (text.IsNullOrEmpty() == true)
            return;

        else if (text.Length <= 6)
        {
            _confirm.text = "";
            _error.ChangeText("비밀번호가 너무 짧습니다");
        }

        else if (_pwd.text.IsNullOrEmpty() == false)
        {
            if (_pwd.text != _confirm.text)
            {
                _error.ChangeText("재확인 비밀번호와 다릅니다", false);
                _isUnmatchPwd = true;
            }
            else
            {
                _isUnmatchPwd = false;
            }
        }
    }

    private void OnDisable()
    {
        _nickPanel.gameObject.SetActive(false);

        _email.onEndEdit.RemoveAllListeners();
        _pwd.onEndEdit.RemoveAllListeners();
        _confirm.onEndEdit.RemoveAllListeners();
    }
}
