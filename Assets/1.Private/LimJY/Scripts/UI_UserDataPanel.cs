using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_UserDataPanel : MonoBehaviour
{
    public static UI_UserDataPanel Instance { get; private set; }
    [SerializeField] TMP_Text userNameText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(ChangeNickName());
    }

    public IEnumerator ChangeNickName()
    {
        FirebaseUser curUser = BackendManager.Auth.CurrentUser;
        DatabaseReference user = BackendManager.DataBase.RootReference.Child("Users");


        user.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snap = task.Result;
            string userID = curUser.UserId;

            if (snap.HasChild(userID))
            {
                string curNick = (string)snap.Child(userID).Child("nickname").Value;
                userNameText.text = curNick;
            }
        });

        yield break;
    }
}
