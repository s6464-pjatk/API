using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
        {
            var rooms = TrainingCenterData.Rooms.AsEnumerable();

            if (minCapacity.HasValue)
            {
                rooms = rooms.Where(room => room.Capacity >= minCapacity.Value);
            }

            if (hasProjector.HasValue)
            {
                rooms = rooms.Where(room => room.HasProjector == hasProjector.Value);
            }

            if (activeOnly == true)
            {
                rooms = rooms.Where(room => room.IsActive);
            }

            return Ok(rooms);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Room> GetById([FromRoute] int id)
        {
            var room = TrainingCenterData.Rooms.FirstOrDefault(room => room.Id == id);

            if (room is null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public ActionResult<IEnumerable<Room>> GetByBuilding([FromRoute] string buildingCode)
        {
            var rooms = TrainingCenterData.Rooms
                .Where(room => string.Equals(
                    room.BuildingCode,
                    buildingCode,
                    StringComparison.OrdinalIgnoreCase));

            return Ok(rooms);
        }

        [HttpPost]
        public ActionResult<Room> Create([FromBody] Room room)
        {
            room.Id = TrainingCenterData.GetNextRoomId();
            TrainingCenterData.Rooms.Add(room);

            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Room> Update([FromRoute] int id, [FromBody] Room updatedRoom)
        {
            var room = TrainingCenterData.Rooms.FirstOrDefault(room => room.Id == id);

            if (room is null)
            {
                return NotFound();
            }

            room.Name = updatedRoom.Name;
            room.BuildingCode = updatedRoom.BuildingCode;
            room.Floor = updatedRoom.Floor;
            room.Capacity = updatedRoom.Capacity;
            room.HasProjector = updatedRoom.HasProjector;
            room.IsActive = updatedRoom.IsActive;

            return Ok(room);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var room = TrainingCenterData.Rooms.FirstOrDefault(room => room.Id == id);

            if (room is null)
            {
                return NotFound();
            }

            var hasReservations = TrainingCenterData.Reservations
                .Any(reservation => reservation.RoomId == id);

            if (hasReservations)
            {
                return Conflict("Cannot delete a room with existing reservations.");
            }

            TrainingCenterData.Rooms.Remove(room);

            return NoContent();
        }
    }
}
