using System;

namespace Common.ViewModel
{
    public class BookAssinationViewModel
    {
        public int BookID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public virtual AssignedPersonViewModel AssignedPerson { get; set; }
    }
}