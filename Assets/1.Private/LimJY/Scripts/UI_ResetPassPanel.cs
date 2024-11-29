using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class UI_ResetPassPanel : MonoBehaviour
{
    [SerializeField] ErrorText ErrorText;
    [SerializeField] TMP_InputField NickInputField;
    [SerializeField] TMP_InputField EmailInputField;

    private DatabaseReference user;


    private void OnEnable()
    {
        user = BackendManager.DataBase.RootReference.Child("Users");

        EmailInputField.text = "";
        NickInputField.text = "";
    }

    //public async void DataConfirm()
    //{
    //    string Nick = NickInputField.text;
    //    string email = EmailInputField.text;


    //    await user.GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        DataSnapshot snapshot = task.Result;

    //        foreach (DataSnapshot child in snapshot.Child("Users").Children)
    //        {
    //            var e = child.Value;
    //        }

    //        if (snapshot.HasChild(userID) == true)
    //        {
    //            string myEmail = (string)snapshot.Child(userID).Child("email").Value;

    //            if (email != myEmail)
    //            {
    //                ErrorText.ChangeText("");
    //            }
    //        }
    //    });
    //}
}
