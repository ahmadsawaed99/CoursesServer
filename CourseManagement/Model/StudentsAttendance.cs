using System;
using CourseManagement.Identity;

namespace CourseManagement.Model
{
    public class StudentsAttendance
    {
        public AppUserModel student { get; set; }

        public bool DoesAttend { get; set; }

        public string AbsenceReason { get; set; }


    }
}
