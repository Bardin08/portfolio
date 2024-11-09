using Portfolio.Api.Configuration;
using Portfolio.Api.Notifications.Emails.Services;

namespace Portfolio.Api.Extensions.DependencyInjection;

public static class FluentEmailServiceExtensions
{
    public static IServiceCollection AddFluentEmailServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var emailOptions = configuration
            .GetRequiredSection(EmailAccountOptions.SectionName)
            .Get<EmailAccountOptions>();

        if (emailOptions == null)
            throw new Exception("Email options not found");

        services
            .AddFluentEmail(emailOptions.AdminEmail, emailOptions.SenderName)
            .AddRazorRenderer()
            .AddSmtpSender(() => new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(
                    emailOptions.AdminEmail,
                    emailOptions.AppPassword
                ),
                Timeout = 20000 // 20 seconds
            });

        services.AddScoped<EmailService>();
        return services;
    }
}