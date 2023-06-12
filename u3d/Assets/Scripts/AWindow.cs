using System;
using System.Collections;
using System.Collections.Generic;
using CodingK_SystemCenter;
using UnityEngine;
using UnityEngine.UI;
using EventType = CodingK_SystemCenter.EventType;

public class AWindow : MonoBehaviour
{
    public Text subText1;
    public Text subText2;
    public Text subText3;
    
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener<string>(EventType.Test_AWindow, ChangeName1,1000);
        EventCenter.AddListener<string>(EventType.Test_AWindow, ChangeName2,200);
        EventCenter.AddListener<string>(EventType.Test_AWindow, ChangeName3,800, "AWindow.Task3");
    }

    void ChangeName(Text target, string x)
    {
        target.text = x;
        Debug.Log(target.name + "   " + DateTime.Now);
    }
    
    void ChangeName1(string x)
    {
        ChangeName(subText1, x);
    }
    
    void ChangeName2(string x)
    {
        ChangeName(subText2, x);
    }
    
    void ChangeName3(string x)
    {
        ChangeName(subText3, x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
