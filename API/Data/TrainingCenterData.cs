using API.Models;

namespace API.Data
{
    public class TrainingCenterData
    {
        public static void Initialize()
        {
        }

        public static List<Room> Rooms { get; } = new()
    {
        new Room
        {
            Id = 1,
            Name = "Sala A101",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 30,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 2,
            Name = "Lab 204",
            BuildingCode = "B",
            Floor = 2,
            Capacity = 24,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 3,
            Name = "Sala C12",
            BuildingCode = "C",
            Floor = 0,
            Capacity = 18,
            HasProjector = false,
            IsActive = true
        },
        new Room
        {
            Id = 4,
            Name = "Audytorium A300",
            BuildingCode = "A",
            Floor = 3,
            Capacity = 80,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 5,
            Name = "Sala B105",
            BuildingCode = "B",
            Floor = 1,
            Capacity = 16,
            HasProjector = false,
            IsActive = false
        }
    };

        public static List<Reservation> Reservations { get; } = new()
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Anna Kowalska",
            Topic = "Warsztaty z HTTP i REST",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 30),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Piotr Nowak",
            Topic = "Konsultacje z C#",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(13, 0),
            EndTime = new TimeOnly(14, 30),
            Status = "planned"
        },
        new Reservation
        {
            Id = 3,
            RoomId = 4,
            OrganizerName = "Maria Zielinska",
            Topic = "Szkolenie z komunikacji",
            Date = new DateOnly(2026, 5, 11),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(11, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 3,
            OrganizerName = "Tomasz Wisniewski",
            Topic = "Podstawy testowania API",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(15, 0),
            EndTime = new TimeOnly(17, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 5,
            RoomId = 1,
            OrganizerName = "Ewa Mazur",
            Topic = "Przeglad projektow",
            Date = new DateOnly(2026, 5, 13),
            StartTime = new TimeOnly(8, 30),
            EndTime = new TimeOnly(10, 0),
            Status = "cancelled"
        }
    };

        public static int GetNextRoomId()
        {
            return Rooms.Count == 0 ? 1 : Rooms.Max(room => room.Id) + 1;
        }

        public static int GetNextReservationId()
        {
            return Reservations.Count == 0 ? 1 : Reservations.Max(reservation => reservation.Id) + 1;
        }
    }
}
