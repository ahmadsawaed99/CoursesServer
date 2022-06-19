using System;
using CourseManagement.Identity;

namespace CourseManagement.Data
{
    public class Student_Class
    {
        public int Id { get; set; }

        public bool IsStudentAttended { get; set; }

        public string UserId { get; set; }

        public AppUser User { get; set; }

        public int ClassId { get; set; }

        public Class _Class { get; set; }
    }
}
