using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishDate { get; set; }
        public decimal CoverPrice { get; set; }
        public CheckInOutStatus? CheckInCheckOut { get; set; }

        public virtual ICollection<BookAssignation> BookAssignations { get; set; }
    }
}
