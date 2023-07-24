using Solana.Unity.Metaplex.NFT.Library;
using Solana.Unity.Metaplex.Utilities;
using Solana.Unity.Programs;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Models;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MintNFT : MonoBehaviour
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
    public void on_click() => mint_nft();

    private bool _loading = false;
    async void mint_nft()
    {
        if (_loading is true) return;

        _loading = true;
        _button.interactable = !_loading;

        GameManager.Instance.MessageDisplayScreen.ShowScreen("Mint NFT (wait)", "Minting...");
        GameManager.Instance.MessageDisplayScreen.CanClose = false;

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


        StringBuilder sb_message = new StringBuilder();
        sb_message.AppendLine("Info:");
        sb_message.AppendLine("Trying to get blockchain hash...");
        GameManager.Instance.MessageDisplayScreen.SetMessageText(sb_message.ToString());


        var blockHash = await Web3.Rpc.GetLatestBlockHashAsync(Solana.Unity.Rpc.Types.Commitment.Confirmed);


        if (!blockHash.WasSuccessful)
        {
            sb_message.AppendLine($"Failed: {blockHash.Reason}");
            GameManager.Instance.MessageDisplayScreen.SetMessageText(sb_message.ToString());
        }

        if (blockHash.WasSuccessful)
        {
            sb_message.AppendLine("Trying to get min balance...");
            GameManager.Instance.MessageDisplayScreen.SetMessageText(sb_message.ToString());

            var minimumRent = await Web3.Rpc.GetMinimumBalanceForRentExemptionAsync(TokenProgram.MintAccountDataSize, 
                Solana.Unity.Rpc.Types.Commitment.Confirmed);
            
            
            if (!minimumRent.WasSuccessful)
            {
                sb_message.AppendLine($"Failed: {minimumRent.Reason}");
                GameManager.Instance.MessageDisplayScreen.SetMessageText(sb_message.ToString());
            }

            if(minimumRent.WasSuccessful)
            {
                sb_message.AppendLine($"Reason: {minimumRent.Reason}");
                sb_message.AppendLine($"Rent: {minimumRent.Result}");
                sb_message.AppendLine($"Status: {minimumRent.HttpStatusCode.ToString()}");
                sb_message.AppendLine($"Error?: {minimumRent?.ErrorData?.Error.ToString()}");

                sb_message.AppendLine("Building transaction and sending it...");
                GameManager.Instance.MessageDisplayScreen.SetMessageText(sb_message.ToString());


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
                .AddInstruction(MetadataProgram.CreateMetadataAccount(
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

                var tx = Transaction.Deserialize(transaction.Build(new List<Account> { Web3.Account, mint }));
                
                var res = await Web3.Wallet.SignAndSendTransaction(tx, true,
                    commitment: Solana.Unity.Rpc.Types.Commitment.Processed);

                sb_message.AppendLine($"Successful?: {res.WasSuccessful}");
                sb_message.AppendLine($"Status: {res.HttpStatusCode}");
                sb_message.AppendLine($"Reason: {res.Reason}");
                sb_message.AppendLine($"Error?: {res?.ErrorData?.Error.ToString()}");
                sb_message.AppendLine($"Key?: {res.Result}");
            }
        }
        
        GameManager.Instance.MessageDisplayScreen.ShowScreen("Mint NFT", sb_message.ToString());
        GameManager.Instance.MessageDisplayScreen.CanClose = true;

        _loading = false;
        _button.interactable = !_loading;
    }
}
