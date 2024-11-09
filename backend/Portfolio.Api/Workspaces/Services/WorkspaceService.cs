using Portfolio.Api.Workspaces.Models;

namespace Portfolio.Api.Workspaces.Services;

internal class WorkspaceService(IGoogleDriveService googleDriveService)
{
    private readonly IGoogleDriveService _googleDriveService = googleDriveService;

    public async Task<CreateWorkspaceResponse> CreateWorkspace(CreateWorkspaceRequest request)
    {
        var folderId = await _googleDriveService.CreateFolder(request.WorkspaceName);
        var folderUrl = await _googleDriveService.GetFolderUrl(folderId);

        var documentId = await _googleDriveService.CreateDocument("Master Document");

        await _googleDriveService.MoveDocumentToFolder(documentId, folderId);
        await _googleDriveService.AddAdminPermission(request.OwnerEmail, folderId);

        var sharableLink = await _googleDriveService.CreateSharableLinks(documentId);
        return new CreateWorkspaceResponse(request.WorkspaceName, folderUrl, sharableLink);
    }
}