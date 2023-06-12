using System.Collections;
using System.Collections.Generic;
using CodingK_SystemCenter;
using UnityEngine;
using UnityEngine.UI;
using EventType = CodingK_SystemCenter.EventType;

public class BWindow : MonoBehaviour
{
    public Text txt;
    public Button btn;
    public Button removeBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            EventCenter.Trigger(EventType.Test_AWindow, txt.text); 
        });
        
        removeBtn.onClick.AddListener(() =>
        { 
            EventCenter.RemoveListener(EventType.Test_AWindow,"AWindow.Task3");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
