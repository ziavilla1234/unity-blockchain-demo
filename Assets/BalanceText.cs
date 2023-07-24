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
        _text_balance.text = "";
    }


    void OnEnable()
    {
        Web3.OnBalanceChange += set_balance_text;
        Web3.OnLogout += on_logout;

    }

    void OnDisable()
    {
        Web3.OnBalanceChange -= set_balance_text;
        Web3.OnLogout -= on_logout;
    }
    void set_balance_text(double sol) => _text_balance.text = $"Balance: {sol} sol";
    void on_logout() => _text_balance.text = "";

    // Update is called once per frame
    void Update()
    {

    }
}
