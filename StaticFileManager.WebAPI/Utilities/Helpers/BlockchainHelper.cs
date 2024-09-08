namespace StaticFileManager.WebAPI.Utilities.Helpers;

public class BlockchainHelper
{
    public IList<FileBasedBlockchainHelper?>? Chain { get; private set; }

    public BlockchainHelper()
    {
        InitializeChain();
        AddGenesisBlock();
    }

    public void InitializeChain()
    {
        Chain = new List<FileBasedBlockchainHelper>();
    }

    public FileBasedBlockchainHelper CreateGenesisBlock()
    {
        return new FileBasedBlockchainHelper(0, DateTime.Now, "Genesis Block", "");
    }

    public void AddGenesisBlock()
    {
        Chain?.Add(CreateGenesisBlock());
    }

    public FileBasedBlockchainHelper? GetLatestBlock()
    {
        return Chain?[^1];
    }

    public void AddBlock(FileBasedBlockchainHelper newBlock)
    {
        newBlock.PreviousHash = GetLatestBlock()?.Hash;
        newBlock.Hash = newBlock.CalculateHash();
        Chain.Add(newBlock);
    }
}