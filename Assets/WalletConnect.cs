using Cysharp.Threading.Tasks;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletConnect : MonoBehaviour
{
    public bool ConnectViaWalletAdapter = true;

    string _pvt_key = "3xQiQAcZMMy5GMndckeJuBkEZNNmA52Bgm3jhLUNigVQfBXquQgdQTDbJdXW11FoPLQK1joBD7dv3MEFmyKAdtYA";
    string _pass = "monopoly";

    Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();

        if(ConnectViaWalletAdapter is false && Application.isEditor is false)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public async void on_click()
    {
        if(ConnectViaWalletAdapter)
        {
            Web3.Instance.LoginWithWalletAdapter();
        }
        else
        {
            await Web3.Instance.CreateAccount(_pvt_key, _pass);
        }
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
        _button.interactable = false;
    }
    void on_logout()
    {
        _button.interactable = true;
    }

    void set_rpc_cluster(int value)
    {
        if (Web3.Instance == null) return;
        Web3.Instance.rpcCluster = (RpcCluster)value;
        Web3.Instance.customRpc = value switch
        {
            (int)RpcCluster.MainNet => "https://rpc.magicblock.gg/solana-mainnet/",
            (int)RpcCluster.TestNet => "https://rpc.magicblock.gg/solana-testnet/",
            _ => "https://rpc.magicblock.gg/solana-devnet/"
        };
        Web3.Instance.webSocketsRpc = value switch
        {
            (int)RpcCluster.MainNet => "wss://red-boldest-uranium.solana-mainnet.quiknode.pro/190d71a30ba3170f66df5e49c8c88870737cd5ce/",
            (int)RpcCluster.TestNet => "wss://polished-omniscient-pond.solana-testnet.quiknode.pro/05d6e963dcc26cb1969f8c8e304dc49ed53324d9/",
            _ => "wss://late-wild-film.solana-devnet.quiknode.pro/8374da8d09b67ce47c9307c1863212e5710b7c69/"
        };
        Web3.Instance.LoginXNFT().AsUniTask().Forget();
    }
}
