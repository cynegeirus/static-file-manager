using Microsoft.AspNetCore.Mvc;
using StaticFileManager.WebAPI.Utilities.Helpers;

namespace StaticFileManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagerController(IWebHostEnvironment environment) : ControllerBase
    {
        private readonly BlockchainHelper _blockchainHelper = new();
        private readonly SecurityHelper _securityHelper = new();

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return NotFound("File not found.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var encryptedFileBytes = _securityHelper.EncryptBytes(fileBytes);

            var fileHash = SecurityHelper.ComputeSha256Hash(encryptedFileBytes);
            var fileExtension = Path.GetExtension(file.FileName);

            var fileName = $"{fileHash}{fileExtension}";
            var filePath = Path.Combine(environment.WebRootPath, "uploads", fileName);

            if (!Directory.Exists(Path.Combine(environment.WebRootPath, "uploads")))
                Directory.CreateDirectory(Path.Combine(environment.WebRootPath, "uploads"));

            await System.IO.File.WriteAllBytesAsync(filePath, encryptedFileBytes);

            var newBlock = new FileBasedBlockchainHelper(_blockchainHelper.Chain!.Count, DateTime.Now, fileHash, fileExtension);
            _blockchainHelper.AddBlock(newBlock);

            return Ok(new { Message = "File uploaded and encrypted, hash blockchain added.", Hash = fileHash, Extension = fileExtension });
        }

        [HttpGet("Download")]
        public IActionResult Download(string hash)
        {
            if (string.IsNullOrEmpty(hash))
                return BadRequest("Invalid hash value.");

            var directoryPath = Path.Combine(environment.WebRootPath, "uploads");
            var filePath = Directory.GetFiles(directoryPath, $"{hash}.*").FirstOrDefault();

            if (filePath == null)
                return NotFound("File not found.");

            var fileExtension = Path.GetExtension(filePath);
            var encryptedFileBytes = System.IO.File.ReadAllBytes(filePath);

            var decryptedFileBytes = _securityHelper.DecryptBytes(encryptedFileBytes);

            var contentType = fileExtension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };

            return File(decryptedFileBytes, contentType, $"{hash}{fileExtension}");
        }
    }
}
