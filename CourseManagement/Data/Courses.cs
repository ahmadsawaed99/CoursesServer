using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseManagement.Identity;
using CourseManagement.Model;

namespace CourseManagement.Data
{
    public class Courses
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartingDate { get; set; }

        public DateTime EndingDate { get; set; }

        public List<CourseTiming> DaysAndHoursOfCourses { get; set; }

        public List<Student_Course> Students { get; set; }

        public List<Class> Classes { get; set; }

    }
}
