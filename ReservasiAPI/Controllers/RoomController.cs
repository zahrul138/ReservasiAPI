using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasiAPI.Repository;
using ReservasiAPI.Repository.Models;

namespace ReservasiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ReservasiDbContext _context;

        public RoomController(ReservasiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            return room;
        }

        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom([FromForm] Room room)
        {
            var form = Request.Form;

            var featuresRaw = form["features"].ToString();
            var amenitiesRaw = form["amenities"].ToString();
            var policiesRaw = form["policies"].ToString();

            room.Features = IsValidJson(featuresRaw) ? featuresRaw : "[]";
            room.Amenities = IsValidJson(amenitiesRaw) ? amenitiesRaw : "[]";
            room.Policies = IsValidJson(policiesRaw) ? policiesRaw : "[]";

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromForm] Room room)
        {
            if (id != room.Id) return BadRequest();

            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom == null) return NotFound();

            var form = Request.Form;

            var featuresRaw = form["features"].ToString();
            var amenitiesRaw = form["amenities"].ToString();
            var policiesRaw = form["policies"].ToString();

            existingRoom.Features = IsValidJson(featuresRaw) ? featuresRaw : existingRoom.Features;
            existingRoom.Amenities = IsValidJson(amenitiesRaw) ? amenitiesRaw : existingRoom.Amenities;
            existingRoom.Policies = IsValidJson(policiesRaw) ? policiesRaw : existingRoom.Policies;

            existingRoom.Title = room.Title ?? existingRoom.Title;
            existingRoom.ShortDescription = room.ShortDescription ?? existingRoom.ShortDescription;
            existingRoom.FullDescription = room.FullDescription ?? existingRoom.FullDescription;
            existingRoom.Price = room.Price != 0 ? room.Price : existingRoom.Price;
            existingRoom.Size = room.Size ?? existingRoom.Size;
            existingRoom.Occupancy = room.Occupancy ?? existingRoom.Occupancy;
            existingRoom.Bed = room.Bed ?? existingRoom.Bed;
            existingRoom.RoomView = room.RoomView ?? existingRoom.RoomView;
            existingRoom.Image1 = room.Image1 ?? "";
            existingRoom.Image2 = room.Image2 ?? "";
            existingRoom.Image3 = room.Image3 ?? "";
            existingRoom.Quantity = room.Quantity != 0 ? room.Quantity : existingRoom.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rooms.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        private static bool IsValidJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            input = input.Trim();
            if ((!input.StartsWith("[") || !input.EndsWith("]")) && (!input.StartsWith("{") || !input.EndsWith("}")))
                return false;
            try
            {
                var obj = System.Text.Json.JsonDocument.Parse(input);
                return true;
            }
            catch
            {
                return false;
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("restore-quantity")]
        public async Task<IActionResult> RestoreRoomQuantity([FromBody] RestoreQuantityDto dto)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Title == dto.RoomType);
            if (room == null)
            {
                return NotFound("Room type not found.");
            }

            room.Quantity += 1;
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        public class RestoreQuantityDto
        {
            public string RoomType { get; set; }
        }
    }
}
