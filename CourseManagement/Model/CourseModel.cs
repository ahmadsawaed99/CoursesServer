using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseManagement.Data;

namespace CourseManagement.Model
{
    public class CourseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartingDate { get; set; }

        public DateTime EndingDate { get; set; }

        public List<CourseTiming> DaysAndHoursOfCourses { get; set; }


    }
}
