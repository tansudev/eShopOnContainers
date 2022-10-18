namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.DomainEventHandlers.OrderCompleted;
public class OrderCompletedDomainEventHandler : INotificationHandler<OrderCompleteDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
    private readonly ILoggerFactory _logger;

    public OrderCompletedDomainEventHandler(IOrderRepository orderRepository,
    ILoggerFactory logger,
    IBuyerRepository buyerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    public async Task Handle(OrderCompleteDomainEvent orderCompleteDomainEvent, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<OrderCompleteDomainEvent>().LogTrace("Order with Id: {OrderId} has been succesfully completed to status {Status} ({Id}))", orderCompleteDomainEvent.Order.Id, nameof(OrderStatus.Completed), OrderStatus.Completed.Id);

        var order = await _orderRepository.GetAsync(orderCompleteDomainEvent.Order.Id);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId.Value.ToString());

        var orderStatusChangedToCompletedIntegrationEvent = new OrderStatusChangedToCompletedIntegrationEvent(order.Id, order.OrderStatus.Name, buyer.Name);

        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStatusChangedToCompletedIntegrationEvent);

    }
}

