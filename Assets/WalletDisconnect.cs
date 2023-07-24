using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WalletDisconnect : MonoBehaviour
{
    Button _button;


    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void on_click()
    {
        Web3.Instance.Logout();
    }

    void OnEnable()
    {
        Web3.OnLogin += on_login;
        Web3.OnLogout += on_logout;
    }

    void OnDisable()
    {
        Web3.OnLogin -= on_login;
        Web3.OnLogout -= on_logout;
    }

    void on_login(Account account)
    {
        _button.interactable = true;
    }
    void on_logout()
    {
        _button.interactable = false;
    }
}
