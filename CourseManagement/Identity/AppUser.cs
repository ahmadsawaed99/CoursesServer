using System;
using System.Collections.Generic;
using CourseManagement.Data;
using Microsoft.AspNetCore.Identity;

namespace CourseManagement.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Adress { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsProf { get; set; }

        public List<Student_Course> Students_Course { get; set; }

        public List<Student_Class> Students_Class { get; set; }

    }
}
