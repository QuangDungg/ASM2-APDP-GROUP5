using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class TeacherModel
    {
        public List<TeacherViewModel> TeachersList { get; set; }
    }

    public class TeacherViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Teacher's Code can be not empty")]
        public string TeacherCode { get; set; }

        [Required(ErrorMessage = "Teacher's Name can be not empty")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Teacher's Birthday can be not empty")]
        public string Birthday { get; set; }

        public string Address { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Teacher's Phone can be not empty")]
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}