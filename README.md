# Static File Manager

A simple Web API for uploading, encrypting, storing, and downloading files with blockchain-based verification.

## Features

- **File Upload**: Upload files, encrypt them, and store securely on the server.
- **File Download**: Decrypt and download files using their unique hash.
- **Blockchain Integration**: Add each uploaded file's hash to a blockchain for integrity verification.

## Technologies Used

- **ASP.NET Core**: Web API framework for building the application.
- **Blockchain**: A simple blockchain implementation for file integrity tracking.
- **File Encryption**: AES encryption for securely storing files.
- **Dependency Injection**: Used to manage dependencies for encryption and blockchain helpers.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- Any modern IDE or text editor (e.g., Visual Studio, Visual Studio Code)

## API Endpoints

### Upload File

- **URL**: `/api/FileManager/Upload`
- **Method**: `POST`
- **Description**: Uploads a file, encrypts it, and stores it on the server. A hash is generated and added to the blockchain.
- **Request Parameters**:
  - `file` (form-data): The file to upload.
- **Response**: 
  - `200 OK`: File uploaded successfully, along with the file hash and extension.
  - `404 Not Found`: File not provided.

### Download File

- **URL**: `/api/FileManager/Download?hash={file_hash}`
- **Method**: `GET`
- **Description**: Downloads the file by its hash, decrypts it, and returns it to the client.
- **Request Parameters**:
  - `hash` (query string): The hash of the file to download.
- **Response**:
  - `200 OK`: File downloaded successfully.
  - `400 Bad Request`: Invalid or missing hash value.
  - `404 Not Found`: File not found.

## Blockchain Implementation

The project includes a basic blockchain implementation to verify the integrity of uploaded files. Each block contains:

- **Index**: The position of the block in the chain.
- **Timestamp**: The time of the block's creation.
- **Hash**: The SHA-256 hash of the encrypted file.
- **Extension**: The file extension of the uploaded file.

New blocks are created and added to the chain each time a file is uploaded.

## Security

This project uses AES encryption for securing files before they are stored on the server. Files are decrypted only when they are requested for download, ensuring secure storage.

## License

This project is licensed under the [MIT License](LICENSE). See the license file for details.

## Issues, Feature Requests or Support

Please use the Issue > New Issue button to submit issues, feature requests or support issues directly to me. You can also send an e-mail to akin.bicer@outlook.com.tr.
