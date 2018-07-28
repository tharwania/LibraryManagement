using System.ComponentModel.DataAnnotations;

namespace Common.ViewModel
{
    public class AssignedPersonViewModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(200, MinimumLength = 1)]
        public string PersonName { get; set; }

        [Required]
        [Display(Name = "Mobile No.")]
        [RegularExpression(@"\d{2}-\d{3}\s\d{4}", ErrorMessage = "Mobile No. should be xx-xxx xxxx format")]
        public string MobileNumber { get; set; }

        [Required]
        [Display(Name = "National ID")]
        [RegularExpression(@"\d{11}", ErrorMessage = "National ID No. should be 11 Digits")]
        public string NationalID { get; set; }
    }
}