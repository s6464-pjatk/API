using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int RoomId { get; set; }

        [Required]
        [MinLength(1)]
        public string OrganizerName { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string Topic { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        [Required]
        [MinLength(1)]
        public string Status { get; set; } = "planned";
    }
}
