using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication11.Data;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    public class RoomController : Controller
    {

        WebApplication11Context _context;


        public RoomController(WebApplication11Context context)
        {
            _context = context;
        }


        public IActionResult GetAvailableRooms(DateTime dateStart, DateTime dateEnd, int persons)
        {


            var reservations = _context.Reservations
                .Where(r => (r.StartDate >= dateStart && r.StartDate <= dateEnd)
                         || (r.EndDate >= dateStart && r.EndDate <= dateEnd)
                         || (r.StartDate < dateStart && r.EndDate > dateEnd))
                .Include(r => r.Rooms) 
                .ToList();





            foreach (var reservation in reservations)
            {
               Console.WriteLine(reservation.Price);
               
            }

            List<Room> uniqueRooms = reservations.SelectMany(r => r.Rooms).ToList();
     




            var availableRooms = _context.Rooms.Where(r => (!uniqueRooms.Contains(r) && r.MaxPeople>=persons)).ToList();
          

            HttpContext.Session.SetString("StartDate", dateStart.ToString("o"));
            HttpContext.Session.SetString("EndDate", dateEnd.ToString("o")); 


            return View(availableRooms);
        }
    }
}
