using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalanceText : MonoBehaviour
{
    // Start is called before the first frame update

    TextMeshProUGUI _text_balance;
    void Start()
    {
        _text_balance = GetComponent<TextMeshProUGUI>();
        Debug.Log("in start");
    }


    void OnEnable()
    {
        //_text_balance.text = $"in enable";
        Web3.OnBalanceChange += set_balance_text;
        
    }



    void OnDisable()
    {
        Web3.OnBalanceChange -= set_balance_text;
        //_text_balance.text = $"in disable";
    }
    void set_balance_text(double sol) => _text_balance.text = $"balance: {sol}";

    // Update is called once per frame
    void Update()
    {

    }
}
