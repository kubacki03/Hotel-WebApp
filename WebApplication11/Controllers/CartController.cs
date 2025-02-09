using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication11.Areas.Identity.Data;
using WebApplication11.Data;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    public class CartController : Controller
    {
        public Dictionary<int, int> Rooms = new Dictionary<int, int>();

        WebApplication11Context _context;


        public CartController(WebApplication11Context context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult AddToCart([FromBody] Dictionary<string, int> data)
        {
            if (data == null || !data.ContainsKey("roomId"))
            {
                return BadRequest();
            }

            int roomId = data["roomId"];
            Dictionary<int, int> rooms = HttpContext.Session.GetDictionary<int, int>("Rooms") ?? new Dictionary<int, int>();

            if (rooms.ContainsKey(roomId))
            {
                rooms[roomId]++;
            }
            else
            {
                rooms[roomId] = 1;
            }

            HttpContext.Session.SetDictionary("Rooms", rooms);
      
            return Ok();
        }


        [HttpGet]
        public IActionResult ShowCart() {

            Dictionary<int, int> roomsInCart = HttpContext.Session.GetDictionary<int, int>("Rooms") ?? new Dictionary<int, int>();

            Dictionary<Room, int> rom = new Dictionary<Room, int>();

            foreach (var x in roomsInCart)
            {
                var room = _context.Rooms.FirstOrDefault(r => r.Id == x.Key);
                rom.Add(room, x.Value);
            }

         
         
            return View(rom);
        }

    }
}
