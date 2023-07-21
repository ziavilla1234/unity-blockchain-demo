using Assembly_CSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public MessageDisplayScreen MessageDisplayScreen;


    public static GameManager Instance { get; private set; }


    public ScreenUIBase CurrentlyOpenedScreen { get; private set; }
    public bool CanShowScreen { get { return CurrentlyOpenedScreen is null; } }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ShowScreen(ScreenUIBase screen)
    {
        if (!CanShowScreen)
            return false;
        
        CurrentlyOpenedScreen = screen;
        CurrentlyOpenedScreen.gameObject.SetActive(true);

        return true;
    }
    public void HideScreen()
    {
        CurrentlyOpenedScreen.gameObject.SetActive(false);
        CurrentlyOpenedScreen = null;
    }
    
}
