using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Panels;

    /*
     LoginPanel : 0
     SignUpPanel : 1
     Re_PassPanel : 2

     DevelooperListPanel : 9
    */

    public void SignUpTabOpen()
    {
        Panels[0].SetActive(false);
        Panels[1].SetActive(true);
    }

    public void SignUpTabClose()
    {
        Panels[1].SetActive(false);
        Panels[0].SetActive(true);    // or 다른 창
    }

    //===

    public void Re_PassTabOpen()
    {
        Panels[0].SetActive(false);
        Panels[2].SetActive(true);
    }

    public void Re_PassTabClose()
    {
        Panels[2].SetActive(false);
        Panels[0].SetActive(true);   // or 다른 창
    }

    //===



    //===

    public void ListTabOpen()
    {
        Panels[0].SetActive(false);
        Panels[9].SetActive(true);
    }

    public void ListTabClose()
    {
        Panels[9].SetActive(false);
        Panels[0].SetActive(true);
    }
}
