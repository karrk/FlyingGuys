using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dummy : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Public_Login");
    }
}
