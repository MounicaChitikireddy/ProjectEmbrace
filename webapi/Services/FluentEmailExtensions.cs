using FluentEmail.Smtp;
using System.Net.Mail;
using System.Net;


namespace webapi.Services
{
    /// <summary>
    /// Creates a fluent email factory using the settings found in Appsettings.json
    /// </summary>
    public static class FluentEmailExtensions
    {
        public static void AddFluentEmail(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            var defaultFromEmail = emailSettings["DefaultFromEmail"];
            var host = emailSettings["Host"];
            var port = emailSettings.GetValue<int>("Port");
            services.AddFluentEmail(defaultFromEmail);
            var sender = services.AddSingleton<FluentEmail.Core.Interfaces.ISender>(x => new SmtpSender
            (new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]),
                EnableSsl = true
            })); 
            
        }
    }
}
