
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Stripe.Checkout;
using WebApplication11.Areas.Identity.Data;
using WebApplication11.Controllers;
using WebApplication11.Data;
using WebApplication11.Models;

public class PaymentController : Controller
{

    private readonly UserManager<User> _userManager;
    private readonly ILogger<HomeController> _logger;
    private readonly WebApplication11Context _context;
    public PaymentController(ILogger<HomeController> logger, UserManager<User> userManager, WebApplication11Context context)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    string API_KEY = Environment.GetEnvironmentVariable("STRIPE_KEY");

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCheckoutSession()
    {


        string startDateStr = HttpContext.Session.GetString("StartDate");
        string endDateStr = HttpContext.Session.GetString("EndDate");
        int days = 0;
        if (!string.IsNullOrEmpty(startDateStr) && !string.IsNullOrEmpty(endDateStr) &&
           DateTime.TryParse(startDateStr, out DateTime startDate) &&
           DateTime.TryParse(endDateStr, out DateTime endDate))
        {
            days = (endDate - startDate).Days;
        }

            Dictionary<int, int> roomsInCart = HttpContext.Session.GetDictionary<int, int>("Rooms") ?? new Dictionary<int, int>();

        float sum = 0;

        foreach (var x in roomsInCart)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == x.Key);
            sum += room.Price * x.Value*days;
        }


        var client = new Stripe.StripeClient(API_KEY);
        var user = await _userManager.GetUserAsync(User);
    
        var lineItems = new List<SessionLineItemOptions>
    {
        new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long?)((sum)*100),
                Currency = "pln",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = "Rezerwacja pokoi w hotelu Quatro"
                }
            },
            Quantity = 1
        }
    };

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card", "klarna", "blik" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = Url.Action("Success", "Payment", null, Request.Scheme),
            CancelUrl = Url.Action("Cancel", "Payment", null, Request.Scheme)
        };

        var service = new SessionService(client);
        Session session = service.Create(options);
        var id = session.Id;

        TempData["SessionId"] = id;


        return Redirect(session.Url);
    }




    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Success()
    {

        var sessionId = TempData["SessionId"]?.ToString();
        if (string.IsNullOrEmpty(sessionId))
        {
            return RedirectToAction("Cancel");
        }
      
        var client = new Stripe.StripeClient(API_KEY);

        var service = new SessionService(client);
        var session = service.Get(sessionId);

        double charge = (double)session.AmountTotal / 100;

        var user = await _userManager.GetUserAsync(User);

        Dictionary<int, int> roomsInCart = HttpContext.Session.GetDictionary<int, int>("Rooms") ?? new Dictionary<int, int>();

       

     

        string startDateStr = HttpContext.Session.GetString("StartDate");
        string endDateStr = HttpContext.Session.GetString("EndDate");

        Reservation reservation = new Reservation();


        if (!string.IsNullOrEmpty(startDateStr) && !string.IsNullOrEmpty(endDateStr) &&
            DateTime.TryParse(startDateStr, out DateTime startDate) &&
            DateTime.TryParse(endDateStr, out DateTime endDate))
        {

            List<Room> rooms = new List<Room>();
            int sum = 0;
            foreach (var room in roomsInCart)
            {
                var r = _context.Rooms.FirstOrDefault(r => r.Id == room.Key);
                rooms.Add(r);
                sum += r.Price;
            }






             reservation = new Reservation
            {
                UserId = user.Id,
                StartDate = startDate,
                EndDate = endDate,
                Rooms = rooms,
                Price = sum
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

          

            HttpContext.Session.Remove("Rooms");
            HttpContext.Session.Remove("StartDate");
            HttpContext.Session.Remove("EndDate");
        }


        
    
        return View(reservation);
    }



    [HttpGet]
    public async Task<IActionResult> Cancel()
    {
        var user = await _userManager.GetUserAsync(User);
 
        return View();
    }
}
