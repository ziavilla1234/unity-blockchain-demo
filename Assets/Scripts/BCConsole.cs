using Solana.Unity.Metaplex.NFT.Library;
using Solana.Unity.Metaplex.Utilities;
using Solana.Unity.Programs;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TABBB.Tools.Console;
using UnityEngine;

public class BCConsole : MonoBehaviour
{
    ValueNode<string> _public_key;
    ValueNode<string> _credits;
    // Start is called before the first frame update
    void Start()
    {
        Panel panel = Console.I.AddPanel("Wallet");
        // Basic text display
        panel.AddInfo("Login", "To update info");
        _public_key = panel.AddInfo("PublicKey", "0");
        _credits = panel.AddInfo("Credits", "0");

        panel.AddButton("Editor Wallet", "Connect", connect_in_editor_wallet);
        panel.AddButton("WebGL Wallet", "Connect", (x) => Web3.Instance.LoginWithWalletAdapter());

        Panel nftPanel = Console.I.AddPanel("NFTs");

        nftPanel.AddButton("NFTs", "Get", get_nfts);
        nftPanel.AddButton("NFTs", "Mint", mint_nft);

        Web3.OnLogin += (account) => { _public_key.SetValue($"{account.PublicKey}"); };
        Web3.OnLogout += () => { _public_key.SetValue(""); _credits.SetValue(""); };
        Web3.OnBalanceChange += (double sol) => { _credits.SetValue($"{sol}"); };
        

        //panel.AddInputField("Input Field", new InputValue("Initial"), InputFieldCallback);

        // Toggles expand and collapse their children by default, similar to the idea of toggling a feature and all 
        // the variables associated to it becoming irrelevant
        //Node toggleNode = panel.AddToggle("Toggle", false, ToggleCallback);
        //toggleNode.AddInfo("Toggle group", "#1");
        //toggleNode.AddInfo("Toggle group", "#2");
        //toggleNode.AddInfo("Toggle group", "#3");
        //toggleNode.AddInfo("Toggle group", "#4");
        //panel.AddSlider("Slider Example", -2f, 4f, 0f, SliderCallback);
        // Filled bars use a color code red-yellow-green based on fill amount (defined on palette)
        //Node nodeHPBars = panel.AddInfo("Enemy Health Bars", "");
        //nodeHPBars.AddFillBar("Enemy #1", 0f, 100f, 10);
        //nodeHPBars.AddFillBar("Enemy #2", 0f, 100f, 30);
        //nodeHPBars.AddFillBar("Enemy #3", 0f, 100f, 50);
        //nodeHPBars.AddFillBar("Enemy #4", 0f, 100f, 70);
        //nodeHPBars.AddFillBar("Enemy #5", 0f, 100f, 90);
        //nodeHPBars.AddFillBar("Enemy #6", 0f, 100f, 100);

        //panel.AddButton("Button", "Press", ButtonCallback);
        //panel.AddVector2("Vector2", new Vector2(Random.value, Random.value), Vector2Callback);
        //panel.AddVector3("Vector3", new Vector3(Random.value, Random.value, Random.value), Vector3Callback);

    }

    bool lastFrame;
    void Update()
    {
        if (Input.GetKey(KeyCode.BackQuote))
        {
            if (lastFrame) return;
            lastFrame = true;
            Console.I.Toggle();
        }
        else
        {
            lastFrame = false;
        }
    }

    async void connect_in_editor_wallet(ButtonValue buttonValue)
    {
        string _pvt_key = "3xQiQAcZMMy5GMndckeJuBkEZNNmA52Bgm3jhLUNigVQfBXquQgdQTDbJdXW11FoPLQK1joBD7dv3MEFmyKAdtYA";
        string _pass = "monopoly";
        Account account = await Web3.Instance.CreateAccount(_pvt_key, _pass);
        Debug.Log(account != null ? "Connected successfully!" : "Failed to connect!");
    }


    bool _loading = false;


    async void get_nfts(ButtonValue buttonValue)
    {
        if(Web3.Account == null)
        {
            Debug.Log("Not Connected!");
            return;
        }

        Debug.Log(_loading ? "Busy try again..." : "Loading NFTs...");
        if (_loading is true) return;
        _loading = true;
        
        var nfts = await Web3.LoadNFTs();

        Debug.Log("Listing NFTs:");

        for (int i = 0; i < nfts.Count; i++)
        {
            Debug.Log($"{i} - {nfts[i].metaplexData.data.mint}");
        }

        _loading = false;
    }

    async void mint_nft(ButtonValue buttonValue)
    {
        if (Web3.Account == null)
        {
            Debug.Log("Not Connected!");
            return;
        }

        Debug.Log(_loading ? "Busy try again..." : "Minting NFT...");
        if (_loading is true) return;
        _loading = true;

        var mint = new Account();
        var associatedTokenAccount = AssociatedTokenAccountProgram
            .DeriveAssociatedTokenAccount(Web3.Account, mint.PublicKey);

        //--------------------------------

        var metadata = new Metadata()
        {
            name = "Test",
            symbol = "TST",
            uri = "https://y5fi7acw5f5r4gu6ixcsnxs6bhceujz4ijihcebjly3zv3lcoqkq.arweave.net/x0qPgFbpex4ankXFJt5eCcRKJzxCUHEQKV43mu1idBU",
            sellerFeeBasisPoints = 0,
            creators = new List<Creator> { new(Web3.Account.PublicKey, 100, true) }
        };


        //------------------------------------


        Debug.Log("Info:");
        Debug.Log("Trying to get blockchain hash...");


        var blockHash = await Web3.Rpc.GetLatestBlockHashAsync();


        if (!blockHash.WasSuccessful)
        {
            Debug.Log($"Failed: {blockHash.Reason}");
        }

        if (blockHash.WasSuccessful)
        {
            Debug.Log("Trying to get min balance...");

            var minimumRent = await Web3.Rpc.GetMinimumBalanceForRentExemptionAsync(TokenProgram.MintAccountDataSize);


            if (!minimumRent.WasSuccessful)
            {
                Debug.Log($"Failed: {minimumRent.Reason}");
            }

            if (minimumRent.WasSuccessful)
            {
                Debug.Log($"Reason: {minimumRent.Reason}");
                Debug.Log($"Rent: {minimumRent.Result}");
                Debug.Log($"Status: {minimumRent.HttpStatusCode.ToString()}");
                Debug.Log($"Error?: {minimumRent?.ErrorData?.Error.ToString()}");

                Debug.Log("Building transaction and sending it...");


                var transaction = new TransactionBuilder()
                .SetRecentBlockHash(blockHash.Result.Value.Blockhash)
                .SetFeePayer(Web3.Account)
                .AddInstruction(
                    SystemProgram.CreateAccount(
                        Web3.Account,
                        mint.PublicKey,
                        minimumRent.Result,
                        TokenProgram.MintAccountDataSize,
                        TokenProgram.ProgramIdKey))
                .AddInstruction(
                    TokenProgram.InitializeMint(
                        mint.PublicKey,
                        0,
                        Web3.Account,
                        Web3.Account))
                .AddInstruction(
                    AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                        Web3.Account,
                        Web3.Account,
                        mint.PublicKey))
                .AddInstruction(
                    TokenProgram.MintTo(
                        mint.PublicKey,
                        associatedTokenAccount,
                        1,
                        Web3.Account))
                .AddInstruction(
                    MetadataProgram.CreateMetadataAccount(
                    PDALookup.FindMetadataPDA(mint),
                    mint.PublicKey,
                    Web3.Account,
                    Web3.Account,
                    Web3.Account.PublicKey,
                    metadata,
                    TokenStandard.NonFungible,
                    true,
                    true,
                    null,
                    metadataVersion: MetadataVersion.V3))
                .AddInstruction(MetadataProgram.CreateMasterEdition(
                    maxSupply: null,
                    masterEditionKey: PDALookup.FindMasterEditionPDA(mint),
                    mintKey: mint,
                    updateAuthorityKey: Web3.Account,
                    mintAuthority: Web3.Account,
                    payer: Web3.Account,
                    metadataKey: PDALookup.FindMetadataPDA(mint),
                    version: CreateMasterEditionVersion.V3)
                );


                //----------------------

                var tx = Solana.Unity.Rpc.Models.Transaction.Deserialize(transaction.Build(new List<Account> { Web3.Account, mint }));

                var res = await Web3.Wallet.SignAndSendTransaction(tx, true);

                Debug.Log($"Successful?: {res.WasSuccessful}");
                Debug.Log($"Status: {res.HttpStatusCode}");
                Debug.Log($"Reason: {res.Reason}");
                Debug.Log($"Error?: {res?.ErrorData?.Error.ToString()}");
                Debug.Log($"Key?: {res.Result}");
            }
        }

        _loading = false;
    }
}
