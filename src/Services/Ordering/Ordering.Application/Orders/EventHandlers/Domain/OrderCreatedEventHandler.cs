using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain;
public class OrderCreatedEventHandler(
    IPublishEndpoint publisherEndpoint,
    IFeatureManager featureManager,
    ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(
        OrderCreatedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handler: {DomainEvent}", domainEvent.GetType().Name);

        if(await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();

            await publisherEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }        
    }
}