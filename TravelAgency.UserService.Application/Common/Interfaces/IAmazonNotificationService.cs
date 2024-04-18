namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IAmazonNotificationService
{
    Task SendPhoneTextMessageAsync(string phoneNumber, string message, CancellationToken cancellationToken);
    Task<string> CreateTopicAsync(string topicName, CancellationToken cancellationToken);
    Task DeleteTopicAsync(string topicArc, CancellationToken cancellationToken);
    Task<string> SubscribeEmailAsync(string topicArn, string email, CancellationToken cancellationToken);
    Task<string> SubscribeEmailForClientAccountAsync(string email, CancellationToken cancellationToken);
    Task<string> SubscribeEmailForTravelAgencyAsync(string email, CancellationToken cancellationToken);
    Task PublishAsync(string topicArn, string message, CancellationToken cancellationToken);
    Task UnsubscribeAsync(string subscriptionArc, CancellationToken cancellationToken);
}
