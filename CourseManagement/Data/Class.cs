using System;
using System.Collections.Generic;

namespace CourseManagement.Data
{
    public class Class
    {
        public int Id { get; set; }

        public DateTime ClassStartingDate { get; set; }

        public DateTime ClassEndingDate { get; set; }

        public List<Student_Class> Students { get; set; }

        public int CourseId { get; set; }

        public Courses Course { get; set; }
    }
}
