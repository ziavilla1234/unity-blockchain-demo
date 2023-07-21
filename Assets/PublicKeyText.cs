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
    }


    void OnEnable()
    {
        Web3.OnLogin += set_public_key_text;
    }

    void OnDisable()
    {
        Web3.OnLogin -= set_public_key_text;
    }

    void set_public_key_text(Account account) => _text_public_key.text = account.PublicKey;

    // Update is called once per frame
    void Update()
    {

    }
}




