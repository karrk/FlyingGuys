using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour, IManager
{
    public static BackendManager Instance { get; private set; }

    private FirebaseApp _app;
    public static FirebaseApp App => Instance._app;

    private FirebaseAuth _auth;
    public static FirebaseAuth Auth => Instance._auth;

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

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase dependencies check success");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
                // Firebase Unity SDK is not safe to use here.
                _app = null;
                _auth = null;
            }
        });
    }
}
