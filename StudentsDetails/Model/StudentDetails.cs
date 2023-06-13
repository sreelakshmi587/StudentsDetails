using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsDetails.Model
{
    public class StudentDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Admission Number is required")]
        public string AdmissionNo { get; set; }
        [Required(ErrorMessage = "Name is required and should have a maximum of 100 characters")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Class is required")]
        public int Class { get; set; }
        [Required(ErrorMessage = "Address is required and should have a maximum of 250 characters")]
        [MaxLength(250)]
        public string Address { get; set; }

    }
}
