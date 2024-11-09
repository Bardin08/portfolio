namespace Portfolio.Api.Workspaces.Models;

public record CreateWorkspaceRequest(
    string WorkspaceName,
    string OwnerEmail);