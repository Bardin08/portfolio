using FluentEmail.Core;
using FluentEmail.Core.Models;
using Portfolio.Api.Models.Requests;
using Portfolio.Api.Notifications.Emails.Models;
using Portfolio.Api.Workspaces.Models;
using Portfolio.Api.Workspaces.Services;

namespace Portfolio.Api.Notifications.Emails.Services;

internal class EmailService(WorkspaceService workspaceService, IFluentEmail fluentEmail)
{
    private readonly WorkspaceService _workspaceService = workspaceService;
    private readonly IFluentEmail _fluentEmail = fluentEmail;
    private readonly string _adminEmail = "vbardin810@gmail.com";

    public async Task SendServiceRequestEmailsAsync(ServicesRequest request)
    {
        // var workspaceRequest = new CreateWorkspaceRequest(request.Name + " Personal Workspace", _adminEmail);
        // var createWorkspaceResponse = await _workspaceService.CreateWorkspace(workspaceRequest);

        var notificationModel = new ServiceRequestNotificationModel
        {
            ClientName = request.Name,
            ConsultantName = "Vladyslav Bardin",
            Location = "Kyiv, Ukraine",
            GoogleDocsUrl = "N/A"
        };

        var customerEmailTask = GetSendEmailTask(
            request.Email,
            EmailConstants.CustomerEmail.Subject.Replace("{0}", notificationModel.ClientName),
            EmailConstants.CustomerEmail.Template,
            notificationModel);

        var adminEmailTask = GetSendEmailTask(
            request.Email,
            EmailConstants.AdminEmail.Subject.Replace("{0}", request.Name),
            EmailConstants.AdminEmail.Template,
            request);

        await Task.WhenAll(customerEmailTask, adminEmailTask);
    }

    private Task<SendResponse> GetSendEmailTask(
        string emailAddress, string subject, string template, object parameters)
        => _fluentEmail
            .To(emailAddress)
            .Subject(subject)
            .UsingTemplate(File.ReadAllText($"./Notifications/Emails/Templates/{template}"), parameters)
            .SendAsync();

    private static class EmailConstants
    {
        public static class AdminEmail
        {
            public const string Subject = "New Service Request from {0}";
            public const string Template = "AdminNotification.cshtml";
        }

        public static class CustomerEmail
        {
            public const string Subject = "Welcome to Your Premium Development Journey - {0}";
            public const string Template = "CustomerNotification.cshtml";
        }
    }
}