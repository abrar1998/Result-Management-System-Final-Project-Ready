using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class StudentViewModel
    {

        [Display(Name = "Name")]
        [Required]
        public string StudentName { get; set; }

        [Display(Name = "Father Name")]
        [Required]
        public string Parentage { get; set; }

        [Display(Name = "Age")]

        [Required]
        public int StudentAge { get; set; }

        [Display(Name = "Email")]

        [Required]
        [EmailAddress]
        public string StudentEmail { get; set; }

        [Display(Name = "Phone")]

        [Required]
        public string StudentPhone { get; set; }

        [Required]
        public string Adhaar { get; set; }

        [Required]
        public string Address { get; set; }

        [Display(Name ="Upload Adhaar/Domicile Photo")]
        [Required(ErrorMessage ="Please Upload any Identity Proof Document")]
        public IFormFile AddressProof { get; set; }


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string Gender {  get; set; }

        [Required]
        public int Course { get; set; }


        [DataType(DataType.Upload)]
        [Display(Name = "Upload Photo")]
        [Required]
        public IFormFile StudentPhoto { get; set; }
    }
}
