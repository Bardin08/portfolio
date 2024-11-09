namespace Portfolio.Api.Workspaces.Models;

internal record CreateWorkspaceResponse(
    string WorkspaceName,
    string FolderUrl,
    string DocumentUrl
);