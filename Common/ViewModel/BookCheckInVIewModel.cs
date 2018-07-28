using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class BookCheckInVIewModel
    {
        public int BoookID { get; set; }
        public int AssinationID { get; set; }


        public AssignedPersonViewModel AssignedPerson { get; set; }

        [Required]
        [Display(Name = "Required Return Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RequiredReturnDate { get; set; }

        [Required]
        [Display(Name = "Actual Return Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActualReturnDate { get; set; }


        [DataType(DataType.Currency)]
        public decimal Penality { get; set; }
    }
}
