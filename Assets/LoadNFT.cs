using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Solana.Unity.SDK;
using System.Text;

public class LoadNFT : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool _loading = false;
    public async void on_click()
    {
        if (_loading is true) return;

        _loading = true;

        GameManager.Instance.MessageDisplayScreen.ShowScreen("NFT LIST (wait)", "Loading...");

        var nfts = await Web3.LoadNFTs();

        
        
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Nfts:");

        for(int i = 0; i < nfts.Count; i++)
        {
            sb.AppendLine($"{i} - {nfts[i].metaplexData.data.mint}");
        }

        GameManager.Instance.MessageDisplayScreen.ShowScreen("NFT LIST", sb.ToString());

        _loading = false;
    }

    
}
