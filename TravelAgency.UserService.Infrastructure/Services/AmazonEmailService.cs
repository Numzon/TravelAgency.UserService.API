using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Domain.Enums;

namespace TravelAgency.UserService.Infrastructure.Services;
public sealed class AmazonEmailService : IAmazonEmailService
{
    private readonly AmazonEmailServiceSettingsDto _settings;
    private readonly IAmazonSimpleEmailServiceV2 _client;

    public AmazonEmailService(IOptions<AmazonEmailServiceSettingsDto> options)
    {
        _settings = options.Value;
        _client = new AmazonSimpleEmailServiceV2Client(RegionEndpoint.GetBySystemName(_settings.Region));
    }

    public async Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken)
    {
        var templateDataAsObject = new Dictionary<string, string>
        {
            { "userEmail", $"{email}" }
        };

        await SendEmailAsync(new Template
        {
            TemplateName = EmailTemplates.WelcomeEmailTemplate,
            TemplateData = JsonConvert.SerializeObject(templateDataAsObject),
        },
        new List<string>
        {
            email
        },
        cancellationToken);
    }

    private async Task SendEmailAsync(Template template, List<string> toAddresses, CancellationToken cancellationToken)
    {
        var request = new SendEmailRequest
        {
            Content = new EmailContent
            {
                Template = template,
            },
            FromEmailAddress = _settings.SourceEmailAddress,
            Destination = new Destination
            {
                ToAddresses = toAddresses
            }
        };

        await _client.SendEmailAsync(request, cancellationToken);
    }
}
