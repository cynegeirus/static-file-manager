using System.Security.Cryptography;
using System.Text;

namespace StaticFileManager.WebAPI.Utilities.Helpers;

public class SecurityHelper
{
    private readonly string _privateKey = "=23muE@63e.HQE+fXGOT8h(1Xca@A-F7";

    public void EncryptFile(string inputFile, string outputFile)
    {
        using var aesAlg = Aes.Create();
        var keyBytes = Encoding.UTF8.GetBytes(_privateKey);
        aesAlg.Key = keyBytes;
        aesAlg.GenerateIV();

        using var outputStream = new FileStream(outputFile, FileMode.Create);
        outputStream.Write(aesAlg.IV, 0, aesAlg.IV.Length);

        using var cryptoStream = new CryptoStream(outputStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write);
        using var inputStream = new FileStream(inputFile, FileMode.Open);
        inputStream.CopyTo(cryptoStream);
    }

    public void DecryptFile(string inputFile, string outputFile)
    {
        using var aesAlg = Aes.Create();
        var keyBytes = Encoding.UTF8.GetBytes(_privateKey);
        aesAlg.Key = keyBytes;

        using var inputStream = new FileStream(inputFile, FileMode.Open);
        var iv = new byte[aesAlg.BlockSize / 8];
        _ = inputStream.Read(iv, 0, iv.Length);

        aesAlg.IV = iv;

        using var cryptoStream = new CryptoStream(inputStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
        using var outputStream = new FileStream(outputFile, FileMode.Create);
        cryptoStream.CopyTo(outputStream);
    }

    public byte[] EncryptBytes(byte[] inputBytes)
    {
        using var aesAlg = Aes.Create();
        var keyBytes = Encoding.UTF8.GetBytes(_privateKey);
        aesAlg.Key = keyBytes;
        aesAlg.GenerateIV();

        using var memoryStream = new MemoryStream();

        memoryStream.Write(aesAlg.IV, 0, aesAlg.IV.Length);

        using var cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write);
        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
        cryptoStream.FlushFinalBlock();

        return memoryStream.ToArray();
    }

    public byte[] DecryptBytes(byte[] inputBytes)
    {
        using var aesAlg = Aes.Create();
        var keyBytes = Encoding.UTF8.GetBytes(_privateKey);
        aesAlg.Key = keyBytes;

        using var memoryStream = new MemoryStream(inputBytes);

        var iv = new byte[aesAlg.BlockSize / 8];
        _ = memoryStream.Read(iv, 0, iv.Length);

        aesAlg.IV = iv;

        using var cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
        using var decryptedStream = new MemoryStream();
        cryptoStream.CopyTo(decryptedStream);

        return decryptedStream.ToArray();
    }

    public static string ComputeSha256Hash(byte[] fileBytes)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(fileBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

}