using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Solana.Unity.SDK;
using System.Text;
using UnityEngine.UI;


public class LoadNFT : MonoBehaviour
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

    void on_login(Solana.Unity.Wallet.Account account) => _button.interactable = true;
    void on_logout() => _button.interactable = false;



    public bool _loading = false;
    public async void on_click()
    {
        if (_loading is true) return;

        _loading = true;
        _button.interactable = !_loading;

        GameManager.Instance.MessageDisplayScreen.ShowScreen("NFT LIST (wait)", "Loading...");
        GameManager.Instance.MessageDisplayScreen.CanClose = false;


        var nfts = await Web3.LoadNFTs();

        
        
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Nfts:");

        for(int i = 0; i < nfts.Count; i++)
        {
            sb.AppendLine($"{i} - {nfts[i].metaplexData.data.mint}");
        }

        GameManager.Instance.MessageDisplayScreen.ShowScreen("NFT LIST", sb.ToString());
        GameManager.Instance.MessageDisplayScreen.CanClose = true;

        _loading = true;
        _button.interactable = !_loading;
    }

    
}
