using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_StageChoice : MonoBehaviour
{
    [SerializeField] private PlayMatch playMath;
    [SerializeField] private int StageNum;


    private void Start()
    {
        playMath = GetComponent<PlayMatch>();
        StageNum = playMath.num;
        

    }


}
