using System;
using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class StudentModel
    {
        public List<StudentViewModel> StudentsList { get; set; }
    }
    public class StudentViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Student Code is required.")]
        [StringLength(10, ErrorMessage = "Student Code cannot exceed 10 characters.")]
        public string StudentCode { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        [StringLength(50, ErrorMessage = "Student Name cannot exceed 50 characters.")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Birthday is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"\d{10}", ErrorMessage = "Phone number must be 10 digits.")]
        [StringLength(10, ErrorMessage = "Phone number cannot exceed 10 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }


    }
}
//using System;
//using System.ComponentModel.DataAnnotations;

//namespace SIMS_SE06205.Models
//{
//    public class StudentViewModel
//    {
//        public string Id { get; set; }

//        [Required(ErrorMessage = "Name is required.")]
//        public string Name { get; set; }

//        [Required(ErrorMessage = "Date of birth is required.")]
//        public DateTime DateOfBirth { get; set; }

//        public string Course { get; set; }
//        public string Address { get; set; }
//        public string PhoneNumber { get; set; }

//        [Required(ErrorMessage = "Email is required.")]
//        [EmailAddress(ErrorMessage = "Invalid email address.")]
//        public string Email { get; set; }
//    }
//    public class StudentModel
//    {
//        public List<StudentViewModel> StudentLists { get; set; }
//    }
//}