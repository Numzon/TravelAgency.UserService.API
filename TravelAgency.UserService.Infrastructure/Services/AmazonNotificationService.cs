using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Infrastructure.Services;
public sealed class AmazonNotificationService : IAmazonNotificationService
{
    private readonly IAmazonSimpleNotificationService _notificationService;
    private readonly AmazonNotificationServiceSettingsDto _settings;

    public AmazonNotificationService(IOptions<AmazonNotificationServiceSettingsDto> options)
    {
        _settings = options.Value;
        _notificationService = new AmazonSimpleNotificationServiceClient(RegionEndpoint.GetBySystemName(_settings.Region));
    }

    public async Task SendPhoneTextMessageAsync(string phoneNumber, string message, CancellationToken cancellationToken)
    {
        var request = new PublishRequest
        {
            PhoneNumber = phoneNumber,
            Message = message
        };

        await _notificationService.PublishAsync(request, cancellationToken);
    }

    public async Task<string> CreateTopicAsync(string topicName, CancellationToken cancellationToken)
    {
        var request = new CreateTopicRequest
        {
            Name = topicName
        };

        var response = await _notificationService.CreateTopicAsync(request, cancellationToken);

        return response.TopicArn;
    }

    public async Task DeleteTopicAsync(string topicArc, CancellationToken cancellationToken)
    {
        var request = new DeleteTopicRequest
        {
            TopicArn = topicArc
        };

        await _notificationService.DeleteTopicAsync(request, cancellationToken);
    }

    public async Task<string> SubscribeEmailAsync(string topicArn, string email, CancellationToken cancellationToken)
    {
        var request = new SubscribeRequest
        {
            TopicArn = topicArn,
            ReturnSubscriptionArn = true,
            Protocol = "email",
            Endpoint = email
        };
        
        var response = await _notificationService.SubscribeAsync(request, cancellationToken);

        return response.SubscriptionArn;
    }

    public async Task PublishAsync(string topicArn, string message, CancellationToken cancellationToken)
    {
        var request = new PublishRequest
        {
            TopicArn = topicArn,
            Message = message
        };

        await _notificationService.PublishAsync(request, cancellationToken);
    }

    public async Task UnsubscribeAsync(string subscriptionArc, CancellationToken cancellationToken)
    {
        var request = new UnsubscribeRequest
        {
            SubscriptionArn = subscriptionArc
        };

        await _notificationService.UnsubscribeAsync(request, cancellationToken);
    }

    public async Task<string> SubscribeEmailForClientAccountAsync(string email, CancellationToken cancellationToken)
    {
        return await SubscribeEmailAsync(_settings.ClientArn, email, cancellationToken);
    }

    public async Task<string> SubscribeEmailForTravelAgencyAsync(string email, CancellationToken cancellationToken)
    {
        return await SubscribeEmailAsync(_settings.TravelAgencyArn, email, cancellationToken);
    }
}
