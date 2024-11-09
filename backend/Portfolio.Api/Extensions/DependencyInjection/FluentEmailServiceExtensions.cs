using Portfolio.Api.Notifications.Emails.Services;

namespace Portfolio.Api.Extensions.DependencyInjection;

public static class FluentEmailServiceExtensions
{
    public static IServiceCollection AddFluentEmailServices(
        this IServiceCollection services)
    {
        if (true)
        {
            // Use MailHog for development
            services
                .AddFluentEmail("vbardin810@gmail.com", "Vladyslav Bardin")
                .AddRazorRenderer()
                .AddSmtpSender(() => new System.Net.Mail.SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(
                        "vbardin810@gmail.com", 
                        "ctht lmxm rmfg zlvk"
                    ),
                    Timeout = 20000 // 20 seconds
                });

            services.AddScoped<EmailService>();
            return services;
        }

        // Production configuration
        // var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        //
        // if (emailSettings == null)
        //     throw new ArgumentNullException(nameof(emailSettings), "Email settings are not configured");
        //
        // services
        //     .AddFluentEmail(emailSettings.DefaultFromEmail, "Vladyslav Bardin")
        //     .AddRazorRenderer()
        //     .AddSmtpSender(options =>
        //     {
        //         options.Host = emailSettings.SmtpHost;
        //         options.Port = emailSettings.SmtpPort;
        //         options.UseSsl = emailSettings.UseSsl;
        //         options.Username = emailSettings.SmtpUsername;
        //         options.Password = emailSettings.SmtpPassword;
        //     });
        //
        // services.AddScoped<EmailService>();
        //
        return services;
    }
}
