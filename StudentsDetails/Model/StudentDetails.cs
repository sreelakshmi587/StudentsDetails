using System.ComponentModel.DataAnnotations;

namespace StudentsDetails.Model
{
    public class StudentDetails
    {
        public int Id { get; set; }
        [Required]
        public string AdmissionNo { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int Class { get; set; }
        [Required]
        [MaxLength(250)]
        public string Address { get; set; }

    }
}
