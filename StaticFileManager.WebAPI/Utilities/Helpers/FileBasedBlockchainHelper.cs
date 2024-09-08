using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace StaticFileManager.WebAPI.Utilities.Helpers;

public class FileBasedBlockchainHelper
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public string Data { get; set; }
    public string FileExtension { get; set; }
    public string? PreviousHash { get; set; }
    public string? Hash { get; set; }

    public FileBasedBlockchainHelper(int index, DateTime timestamp, string data, string fileExtension, string? previousHash = "")
    {
        Index = index;
        Timestamp = timestamp;
        Data = data;
        FileExtension = fileExtension;
        PreviousHash = previousHash;
        Hash = CalculateHash();
    }

    public string? CalculateHash()
    {
        using var sha256 = SHA256.Create();
        var rawData = Index + Timestamp.ToString(CultureInfo.InvariantCulture) + Data + FileExtension + PreviousHash;
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }



}