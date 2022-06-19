using System;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Model
{
    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [Compare("ConfirmPassword")]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
