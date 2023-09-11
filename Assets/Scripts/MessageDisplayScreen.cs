using Assembly_CSharp;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplayScreen : ScreenUIBase
{
    
    public TMP_InputField MessageInputField;

    public TextMeshProUGUI TitleText;

    

    // Start is called before the first frame update
    void Start()
    {
        this.Id = ScreenUIBase.MessageDisplayScreenId;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void on_close_click() => this.HideScreen();


    public void SetMessageText(string text) => MessageInputField.text = text;
    public void SetTitleText(string text) => TitleText.text = text;
    

    public void ShowScreen(string title, string text)
    {
        this.SetTitleText(title);
        this.SetMessageText(text);
        this.ShowScreen();
    }
}
