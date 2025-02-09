using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication11.Areas.Identity.Data;
using WebApplication11.Data;

namespace WebApplication11.Controllers
{
    public class ReservationController : Controller 
    {
        private readonly ILogger<HomeController> _logger;
        public readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly WebApplication11Context _context;

        public ReservationController(ILogger<HomeController> logger, SignInManager<User> signInManager, UserManager<User> userManager, WebApplication11Context context)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<ActionResult> GetUserReservation()
        {
            var user = await _userManager.GetUserAsync(User);

            var reservations = _context.Reservations.Where(r => r.UserId==user.Id).ToList();


            return View(reservations);
        }
        
       
    }
}
