using System;
using System.ComponentModel.DataAnnotations;
namespace CourseManagement.Identity
{
    public class SignUp
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Adress { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsProf { get; set; }

    }
}
