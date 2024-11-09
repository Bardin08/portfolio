using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Options;
using Portfolio.Api.Configuration;
using Portfolio.Api.Models.Requests;
using Portfolio.Api.Notifications.Emails.Models;

namespace Portfolio.Api.Notifications.Emails.Services;

internal class EmailService(IFluentEmail fluentEmail, IOptions<EmailAccountOptions> emailAccountOptions)
{
    private readonly IFluentEmail _fluentEmail = fluentEmail;
    private readonly string _adminEmail = emailAccountOptions.Value.AdminEmail;

    public async Task SendServiceRequestEmailsAsync(ServicesRequest request)
    {
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
            _adminEmail,
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

    public async Task SendMentoringRequestEmailsAsync(MentoringRequest req)
    {
        var description = $"""
                          Preferred Contact: {req.MessengerType}
                          Mentoring Direction: {req.MentoringDirection}
                          
                          Expectations: {req.GoalsAndExpectations}
                          """;

        var serviceRequest = new ServicesRequest(
            Name: req.Name,
            Email: req.Email,
            Description: description,
            Link: string.Empty);

        await GetSendEmailTask(
            _adminEmail,
            EmailConstants.AdminEmail.Subject.Replace("{0}", req.Name),
            EmailConstants.AdminEmail.Template,
            serviceRequest);
    }
}