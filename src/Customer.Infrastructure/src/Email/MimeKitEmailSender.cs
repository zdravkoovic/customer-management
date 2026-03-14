using Customer.Core.src.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Customer.Infrastructure.src.Email;

public class MimeKitEmailSender(
    ILogger<MimeKitEmailSender> logger,
    IOptions<MailserverConfiguration> mailserverOptions
) : IEmailSender
{
    private readonly ILogger<MimeKitEmailSender> _logger = logger;
    private readonly MailserverConfiguration _mailserverConfiguration = mailserverOptions.Value;
    public async Task SendEmailAsync(string to, string from, string subject, string body)
    {
        _logger.LogWarning("Sending email to {to} from {from} with subject {subject} using {type}.", to, from, subject, this.ToString());

        try
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_mailserverConfiguration.Hostname,
                _mailserverConfiguration.Port, false
            );
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            await client.SendAsync(message);

            await client.DisconnectAsync(
                true
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            throw;
        }
    }
}