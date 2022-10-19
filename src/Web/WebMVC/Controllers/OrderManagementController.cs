namespace WebMVC.Controllers;

[Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
public class OrderManagementController : Controller
{
    private IOrderingService _orderSvc;
    private readonly IIdentityParser<ApplicationUser> _appUserParser;
    public OrderManagementController(IOrderingService orderSvc, IIdentityParser<ApplicationUser> appUserParser)
    {
        _appUserParser = appUserParser;
        _orderSvc = orderSvc;
    }

    public async Task<IActionResult> Index()
    {
        var user = _appUserParser.Parse(HttpContext.User);
        var vm = await _orderSvc.GetMyOrders(user);

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> OrderProcess(string orderId, string actionCode)
    {
        if (OrderProcessAction.Ship.Code == actionCode)
        {
            await _orderSvc.ShipOrder(orderId);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Complete(string orderId,string actionCode)
    {
        if (OrderProcessAction.Complete.Code == actionCode)
        {
            await _orderSvc.CompleteOrder(orderId);
        }
        //Redirect to historic list.
        return RedirectToAction("Index");
    }
}
