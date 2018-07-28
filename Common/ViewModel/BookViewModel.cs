using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class BookViewModel
    {
        public string BookTitle { get; set; }

        public string ISBN { get; set; }

        public int PublishYear { get; set; }

        public decimal CoverPrice { get; set; }

        public string CheckInOutStatus { get; set; }

        public int BookID { get; set; }
    }
}
