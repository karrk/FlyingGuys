using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BackendManager : MonoBehaviour, IManager
{
    public static BackendManager Instance { get; private set; }

    private FirebaseApp _app;
    public static FirebaseApp App => Instance._app;

    private FirebaseAuth _auth;
    public static FirebaseAuth Auth => Instance._auth;

    private FirebaseDatabase _db;
    public static FirebaseDatabase DataBase => Instance._db;

    public static bool RealUsing = false;

    public void Init()
    {
        Instance = this;
        FirebaseInit();
    }

    private void FirebaseInit()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                _app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                _db = FirebaseDatabase.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase dependencies check success");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
                // Firebase Unity SDK is not safe to use here.
                _app = null;
                _auth = null;
                _db = null;
            }
        });
    }

    public async Task ConvertUseState(bool used)
    {
        DatabaseReference user = _db.RootReference.Child("Users").Child(Auth.CurrentUser.UserId);

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["/used"] = used;

        await user.UpdateChildrenAsync(dic);
    }

    public async void Logout()
    {
        await ConvertUseState(false);
    }

    public class User
    {
        public string nickname;
        public string email;
        public bool used;

        public User(string name, string email)
        {
            this.nickname = name;
            this.email = email;
        }
    }

    private void OnDisable()
    {
        if(Auth.CurrentUser != null && RealUsing == true)
        {
            Logout();
        }
    }
}
