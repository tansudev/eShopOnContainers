namespace Microsoft.eShopOnContainers.Services.Ordering.Domain.Events;

public class OrderCompleteDomainEvent : INotification
{
    public Order Order { get; }

    public OrderCompleteDomainEvent(Order order)
    {
        Order = order;
    }
}

