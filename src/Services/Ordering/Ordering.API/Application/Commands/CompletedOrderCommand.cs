namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;

public class CompletedOrderCommand : IRequest<bool>
{
    [DataMember]
    public int OrderNumber { get; private set; }

    public CompletedOrderCommand(int orderNumber)
    {
        OrderNumber = orderNumber;

    }
}

