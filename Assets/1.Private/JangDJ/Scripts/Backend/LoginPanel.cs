using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using System;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using Firebase.Database;
using System.Threading.Tasks;
using Photon.Pun;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _pwd;
    [SerializeField] private ErrorText _error;

    [SerializeField] private NicknamePanel _nickPanel;

    private void OnEnable()
    {
        _email.onEndEdit.AddListener(CheckEmailpattern);
    }

    private void OnDisable()
    {
        _email.onEndEdit.RemoveAllListeners();
        _email.text = "";
        _pwd.text = "";
    }

    private void CheckEmailpattern(string text)
    {
        if (text.IsNullOrEmpty() == true)
            return;

        else if (!text.Contains('.') || !text.Contains('@'))
        {
            _email.text = "";
            _error.ChangeText("옳바른 이메일을 입력해주세요");
        }
    }

    public async void LoginLogic()
    {
        bool success = false;

        await BackendManager.Auth.SignInWithEmailAndPasswordAsync(_email.text, _pwd.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Exception excption = task.Exception.GetBaseException();

                    if (excption is FirebaseException exp)
                    {
                        var errorCode = (Firebase.Auth.AuthError)exp.ErrorCode;

                        Debug.Log(errorCode);

                        switch (errorCode)
                        {
                            case AuthError.InvalidEmail:
                                _error.ChangeText("유효하지 않은 이메일입니다");
                                break;

                            case AuthError.WrongPassword:
                                _error.ChangeText("비밀번호를 확인해주세요");
                                break;

                            #region 에러코드
                            case AuthError.EmailAlreadyInUse:
                                break;
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

                AuthResult result = task.Result;

                Debug.Log("로그인 확인중");

                success = true;
            });

        if(success == true)
            await CheckUser();
    }

    private async Task CheckUser()
    {
        string uid = BackendManager.Auth.CurrentUser.UserId;
        DatabaseReference users = BackendManager.DataBase.RootReference.Child("Users");
        
        bool isUsing = false;

        await users.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted == false)
                return;

            DataSnapshot snapshot = task.Result;

            if(snapshot.HasChild(uid) == false)
            {
                Debug.Log("신규 가입자");

                _nickPanel.gameObject.SetActive(true);
                _nickPanel.RegistedEmail = _email.text;
                _nickPanel.LoginPanel = this;

                return;
            }
            else
            {
                isUsing = (bool)snapshot.Child(uid).Child("used").Value;

                if (isUsing == true)
                {
                    _error.ChangeText("사용중인 계정입니다");
                }

                else
                {
                    BackendManager.Instance.ConvertUseState(true);
                    BackendManager.RealUsing = true;
                    LoadNextScene();
                }
            }
        });
    }

    public void LoadNextScene()
    {
        _nickPanel.gameObject.SetActive(false);
        PhotonNetwork.LoadLevel("Public_Menu");
    }
}
