using Solana.Unity.Metaplex.NFT.Library;
using Solana.Unity.Metaplex.Utilities;
using Solana.Unity.Programs;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Models;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MintNFT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void on_click() => mint_nft();

    private bool _loading = false;
    async void mint_nft()
    {
        if (_loading is true) return;

        _loading = true;

        GameManager.Instance.MessageDisplayScreen.ShowScreen("Mint NFT (wait)", "Minting...");

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



        var blockHash = await Web3.Rpc.GetLatestBlockHashAsync();

        var minimumRent = await Web3.Rpc.GetMinimumBalanceForRentExemptionAsync(TokenProgram.MintAccountDataSize);

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
            version: CreateMasterEditionVersion.V3
        )
        );


        //----------------------

        var tx = Transaction.Deserialize(transaction.Build(new List<Account> { Web3.Account, mint }));
        var res = await Web3.Wallet.SignAndSendTransaction(tx);

        
        GameManager.Instance.MessageDisplayScreen.ShowScreen("Mint NFT", res.Result);

        _loading = false;
    }
}
