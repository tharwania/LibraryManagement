using System;

namespace DAL.Models
{
    public class BookAssignation
    {
        public int ID { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public int BookID { get; set; }
        public int AssgnedPersonID { get; set; }

        public virtual Book Book { get; set; }
        public virtual AssignedPerson AssignedPerson { get; set;}
    }
}