using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
        {
            var reservations = TrainingCenterData.Reservations.AsEnumerable();

            if (date.HasValue)
            {
                reservations = reservations.Where(reservation => reservation.Date == date.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                reservations = reservations.Where(reservation => string.Equals(
                    reservation.Status,
                    status,
                    StringComparison.OrdinalIgnoreCase));
            }

            if (roomId.HasValue)
            {
                reservations = reservations.Where(reservation => reservation.RoomId == roomId.Value);
            }

            return Ok(reservations);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Reservation> GetById([FromRoute] int id)
        {
            var reservation = TrainingCenterData.Reservations.FirstOrDefault(reservation => reservation.Id == id);

            if (reservation is null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult<Reservation> Create([FromBody] Reservation reservation)
        {
            var roomCheck = ValidateRoomForReservation(reservation.RoomId);

            if (roomCheck is not null)
            {
                return roomCheck;
            }

            if (HasTimeConflict(reservation))
            {
                return Conflict("Reservation overlaps with an existing reservation for this room.");
            }

            reservation.Id = TrainingCenterData.GetNextReservationId();
            TrainingCenterData.Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Reservation> Update([FromRoute] int id, [FromBody] Reservation updatedReservation)
        {
            var reservation = TrainingCenterData.Reservations.FirstOrDefault(reservation => reservation.Id == id);

            if (reservation is null)
            {
                return NotFound();
            }

            var roomCheck = ValidateRoomForReservation(updatedReservation.RoomId);

            if (roomCheck is not null)
            {
                return roomCheck;
            }

            updatedReservation.Id = id;

            if (HasTimeConflict(updatedReservation, id))
            {
                return Conflict("Reservation overlaps with an existing reservation for this room.");
            }

            reservation.RoomId = updatedReservation.RoomId;
            reservation.OrganizerName = updatedReservation.OrganizerName;
            reservation.Topic = updatedReservation.Topic;
            reservation.Date = updatedReservation.Date;
            reservation.StartTime = updatedReservation.StartTime;
            reservation.EndTime = updatedReservation.EndTime;
            reservation.Status = updatedReservation.Status;

            return Ok(reservation);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var reservation = TrainingCenterData.Reservations.FirstOrDefault(reservation => reservation.Id == id);

            if (reservation is null)
            {
                return NotFound();
            }

            TrainingCenterData.Reservations.Remove(reservation);

            return NoContent();
        }

        private static ActionResult? ValidateRoomForReservation(int roomId)
        {
            var room = TrainingCenterData.Rooms.FirstOrDefault(room => room.Id == roomId);

            if (room is null)
            {
                return new NotFoundObjectResult("Room does not exist.");
            }

            if (!room.IsActive)
            {
                return new BadRequestObjectResult("Cannot create a reservation for an inactive room.");
            }

            return null;
        }

        private static bool HasTimeConflict(Reservation reservation, int? ignoredReservationId = null)
        {
            return TrainingCenterData.Reservations.Any(existingReservation =>
                existingReservation.Id != ignoredReservationId
                && existingReservation.RoomId == reservation.RoomId
                && existingReservation.Date == reservation.Date
                && !IsCancelled(existingReservation)
                && !IsCancelled(reservation)
                && reservation.StartTime < existingReservation.EndTime
                && reservation.EndTime > existingReservation.StartTime);
        }

        private static bool IsCancelled(Reservation reservation)
        {
            return string.Equals(reservation.Status, "cancelled", StringComparison.OrdinalIgnoreCase);
        }

    }
}
