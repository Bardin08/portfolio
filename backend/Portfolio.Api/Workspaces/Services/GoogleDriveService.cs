using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;

namespace Portfolio.Api.Workspaces.Services;

internal interface IGoogleDriveService
{
    Task<string> CreateFolder(string folderName);
    Task<string> GetFolderUrl(string folderId);
    Task<string> CreateDocument(string documentTitle);
    Task MoveDocumentToFolder(string documentId, string folderId);
    Task AddAdminPermission(string email, string folderId, string role = "writer");
    Task<string> CreateSharableLinks(string documentId);
}

public class GoogleDriveService(
    DriveService driveService,
    DocsService docsService) : IGoogleDriveService
{
    private readonly DriveService _driveService = driveService;
    private readonly DocsService _docsService = docsService;

    public async Task<string> CreateFolder(string folderName)
    {
        var folderMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = folderName,
            MimeType = "application/vnd.google-apps.folder"
        };

        var folderRequest = _driveService.Files.Create(folderMetadata);
        folderRequest.Fields = "id";
        var folder = await folderRequest.ExecuteAsync();
        return folder.Id;
    }

    public async Task<string> GetFolderUrl(string folderId)
    {
        var folderLinkRequest = _driveService.Files.Get(folderId);
        folderLinkRequest.Fields = "webViewLink";
        var folderLink = await folderLinkRequest.ExecuteAsync();
        return folderLink.WebViewLink;
    }

    public async Task<string> CreateDocument(string documentTitle)
    {
        var document = new Document
        {
            Title = documentTitle
        };

        var doc = await _docsService.Documents.Create(document).ExecuteAsync();
        return doc.DocumentId;
    }

    public async Task MoveDocumentToFolder(string documentId, string folderId)
    {
        var updateRequest = _driveService.Files.Update(new Google.Apis.Drive.v3.Data.File(), documentId);
        updateRequest.AddParents = folderId;
        await updateRequest.ExecuteAsync();
    }

    public async Task AddAdminPermission(string email, string folderId, string role = "writer")
    {
        var adminPermission = new Permission
        {
            Type = "user",
            Role = role,
            EmailAddress = email
        };

        var adminPermissionRequest = _driveService.Permissions.Create(adminPermission, folderId);
        adminPermissionRequest.Fields = "id";
        await adminPermissionRequest.ExecuteAsync();
    }

    public async Task<string> CreateSharableLinks(string documentId)
    {
        var permission = new Permission
        {
            Type = "anyone",
            Role = "writer"
        };

        var permissionRequest = _driveService.Permissions.Create(permission, documentId);
        permissionRequest.Fields = "id";
        var permissionsRequestTask = permissionRequest.ExecuteAsync();

        var fileRequest = _driveService.Files.Get(documentId);
        fileRequest.Fields = "webViewLink";
        var fileRequestTask = fileRequest.ExecuteAsync();
        
        await Task.WhenAll(permissionsRequestTask, fileRequestTask);
        return fileRequestTask.Result.WebViewLink;
    }
}
