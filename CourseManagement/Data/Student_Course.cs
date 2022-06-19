using System;
using System.ComponentModel.DataAnnotations;
using CourseManagement.Identity;

namespace CourseManagement.Data
{
    public class Student_Course
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public Courses Course { get; set; }

        public string UserId { get; set; }

        public AppUser User { get; set; }
    }
}
