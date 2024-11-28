using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static BackendManager;

public class NicknamePanel : MonoBehaviour
{
    [HideInInspector] public string RegistedEmail;
    [SerializeField] private TMP_InputField _nick;
    [SerializeField] private ErrorText _error;
    public LoginPanel LoginPanel;

    public async void Regist()
    {
        DataSnapshot snapshot = await GetSnapShot();
        bool result = FindMatchNickname(snapshot);

        if(result == true)
        {
            _error.ChangeText("중복된 닉네임입니다");
        }
        else
        {
            RegistNewUserData(_nick.text, RegistedEmail);
            BackendManager.RealUsing = true;
            await UpdateProfile();
            await BackendManager.Instance.ConvertUseState(true);
            LoginPanel.LoadNextScene();
        }
    }

    private async Task UpdateProfile()
    {
        UserProfile newProfile = new UserProfile();
        newProfile.DisplayName = _nick.text;

        await BackendManager.Auth.CurrentUser.UpdateUserProfileAsync(newProfile);

        Debug.Log(BackendManager.Auth.CurrentUser.DisplayName);
    }

    private async Task<DataSnapshot> GetSnapShot()
    {
        DataSnapshot data = null;

        await BackendManager.DataBase.GetReference("Users").GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {   if (task.IsCompleted)
                {
                    data = task.Result;
                }
            });

        return data;
    }

    private bool FindMatchNickname(DataSnapshot data)
    {
        foreach (var userInfo in data.Children)
        {
            if (userInfo.ChildrenCount <= 0)
                continue;

            foreach (var item in userInfo.Children)
            {
                if(item.Key == "nickname")
                {
                    if ((string)item.Value == _nick.text)
                        return true;
                }
            }
        }

        return false;
    }

    private void RegistNewUserData(string name, string email)
    {
        User user = new User(name, email);
        string json = JsonUtility.ToJson(user);

        BackendManager.DataBase.RootReference.Child("Users").Child(BackendManager.Auth.CurrentUser.UserId).SetRawJsonValueAsync(json);
    }

    private void OnDisable()
    {
        RegistedEmail = null;
        _nick.text = "";
    }
}
