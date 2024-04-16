using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Domain.Enums;

namespace TravelAgency.UserService.Infrastructure.Services;
public sealed class AmazonSimpleEmailService : IAmazonSimpleEmailService
{
    private readonly AmazonSimpleEmailServiceSettingsDto _settings;

    public AmazonSimpleEmailService(IOptions<AmazonSimpleEmailServiceSettingsDto> options)
    {
        _settings = options.Value;
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
        var client = new AmazonSimpleEmailServiceV2Client(RegionEndpoint.GetBySystemName(_settings.Region));

        var request = new SendEmailRequest
        {
            Content = new EmailContent
            {
                Template = template
            },
            FromEmailAddress = _settings.SourceEmailAddress,
            Destination = new Destination
            {
                ToAddresses = toAddresses
            }
        };

        await client.SendEmailAsync(request, cancellationToken);
    }
}
