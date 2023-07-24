using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PublicKeyText : MonoBehaviour
{
    // Start is called before the first frame update

    TextMeshProUGUI _text_public_key;
    void Start()
    {
        _text_public_key = GetComponent<TextMeshProUGUI>();
        _text_public_key.text = "";
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

    void on_login(Account account) => _text_public_key.text = $"Key: {account.PublicKey}";
    void on_logout() => _text_public_key.text = "";

    // Update is called once per frame
    void Update()
    {

    }
}




