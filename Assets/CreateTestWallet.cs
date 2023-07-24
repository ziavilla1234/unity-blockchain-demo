using Cysharp.Threading.Tasks;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTestWallet : MonoBehaviour
{
    string _pvt_key = "3xQiQAcZMMy5GMndckeJuBkEZNNmA52Bgm3jhLUNigVQfBXquQgdQTDbJdXW11FoPLQK1joBD7dv3MEFmyKAdtYA";
    string _pass = "monopoly";

    Account _account = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void ConnectWallet()
    {
        _account = await Web3.Instance.CreateAccount(_pvt_key, _pass);
    }
    
}
