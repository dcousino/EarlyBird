using System;

namespace EarlyBird.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ClubId { get; set; }
        public string UserName { get; set; }
        public string EventId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}