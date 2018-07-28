using System;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModel
{
    public class BookAssinationViewModel
    {
        public int BookID { get; set; }

        [Required]
        [Display(Name = "Check-In Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [Display(Name = "Check-Out Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CheckOutDate { get; set; }

        [Required]
        [Display(Name = "Person Assigned")]
        public virtual AssignedPersonViewModel AssignedPerson { get; set; }
    }
}