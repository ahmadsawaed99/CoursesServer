using System;
namespace CourseManagement.Data
{
    public class CourseTiming
    {
        public int Id { get; set; }

        public DayOfWeek Day { get; set; }

        public int StartClassHour { get; set; }

        public int EndClassHour { get; set; }

        public int CourseId { get; set; }

        public Courses Course { get; set; }

    }
}
