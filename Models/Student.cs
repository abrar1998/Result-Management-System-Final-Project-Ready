using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }


        [Display(Name = "Name")]
        [Required]
        public string StudentName { get; set; }

        [Display(Name = "Father Name")]
        [Required]
        public string Parentage { get; set; }

        [Required]
        public int StudentAge { get; set; }

        [Required]
        [EmailAddress]
        public string StudentEmail { get; set; }

        [Required]
        public string StudentPhone { get; set; }

        [Required]
        public string Adhaar { get; set; }

        [Required]
        public string Gender { get; set; }


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DOB { get; set; }

        [DataType(DataType.Date)]
        public DateOnly RegistrationDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        public string StudentPhoto { get; set; }


        ///property for Address Proof
        public string Address {  get; set; }

        public string AddressProof {  get; set; }

        public int Course { get; set; }
        [ForeignKey("Course")]
        public Course CourseList { get; set; }

        public List<FeeDetails> FeeDetails { get; set; }

        ///property for Address Proof
        //public string Address {  get; set; }

    }
}
