using System;
namespace CourseManagement.Model
{
    public class ClassModel
    {
        public int Id { get; set; }

        public DayOfWeek Day { get; set; }

        public DateTime Date { get; set; }

        public string FromHour { get; set; }

        public string UntilHour { get; set; }
    }
}
