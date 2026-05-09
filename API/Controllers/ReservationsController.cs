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

    }
}
