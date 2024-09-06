using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : UIWindow
{
    public Text txtTitle;
    void Start()
    {
        
    }

    public void SetTitle(string title)
    { 
        txtTitle.text = title;
    }

    void Update()
    {
        
    }
}
