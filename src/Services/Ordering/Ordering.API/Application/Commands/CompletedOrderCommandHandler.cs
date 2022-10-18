namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;

public class CompletedOrderCommandHandler : IRequestHandler<CompletedOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public CompletedOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(CompletedOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetCompletedStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class CompleteOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CompletedOrderCommand, bool>
{
    public CompleteOrderIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, ILogger<IdentifiedCommandHandler<CompletedOrderCommand, bool>> logger) : base(mediator, requestManager, logger)
    {
    }
    protected override bool CreateResultForDuplicateRequest()
    {
        return true;
    }
}