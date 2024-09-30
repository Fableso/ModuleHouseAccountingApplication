using Application.Abstractions;
using Application.Exceptions;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace Application.Services;

public class SendGridEmailSender : IEmailSender
{
    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public SendGridEmailSender(IConfiguration configuration)
    {
        _apiKey = configuration["SendGridEmailSender:ApiKey"] ?? throw new ConfigurationException("SendGrid API key is not configured");
        _fromEmail = configuration["SendGridEmailSender:FromEmail"] ?? throw new ConfigurationException("SendGrid from email is not configured"); 
        _fromName = configuration["SendGridEmailSender:FromName"] ?? throw new ConfigurationException("SendGrid from name is not configured");
    }
    
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(email);
        var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, message, message);
        await client.SendEmailAsync(sendGridMessage);
    }
}