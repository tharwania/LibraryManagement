using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class BookViewModel
    {
        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }

        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Display(Name = "Publish Year")]
        public int PublishYear { get; set; }

        [Display(Name = "Cover Price")]
        public decimal CoverPrice { get; set; }

        [Display(Name = "Status")]
        public string CheckInOutStatus { get; set; }

        public int BookID { get; set; }
    }
}
