using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Repository
{
    public class ClassRepository : IClassRepository
    {
        private AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentsAttendance>> GetStudentsOfClassAsync(int classId)
        {
            var students_class = await _context.Students_Class
                .Where(s => s.ClassId == classId)
                .OrderBy(s => s.UserId).ToListAsync();

            var StudentsOfTheClass = new List<StudentsAttendance>();

            foreach (var record in students_class)
            {
                var student = new StudentsAttendance()
                {
                    student = await _context.Users.Where(s => s.Id == record.UserId).Select(s => new AppUserModel()
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Adress = s.Adress,
                        Email = s.Email

                    }).FirstAsync(),

                    DoesAttend = record.IsStudentAttended,
                    AbsenceReason = ""
                };
                StudentsOfTheClass.Add(student);
            }

            return StudentsOfTheClass;
        }

        public async Task <List<ClassModel>> GetCourseClassesAsync (int courseId)
        {
            var classes = await _context.Classes.Where(c => c.CourseId == courseId).Select(c => new ClassModel()
            {
                Id = c.Id,
                Day = c.ClassStartingDate.DayOfWeek,
                Date = c.ClassStartingDate,
                FromHour =  c.ClassStartingDate.Hour + ":00",
                UntilHour = c.ClassEndingDate.Hour + ":00"

            }).OrderBy(c=>c.Date).ToListAsync();

            return classes;
        }

        public async Task<List<ClassModel>> GetStudentCourseClassesAsync(int courseId)
        {
            var classes = await _context.Classes.Where(c => c.CourseId == courseId && c.ClassStartingDate < DateTime.Now).Select(c => new ClassModel()
            {
                Id = c.Id,
                Day = c.ClassStartingDate.DayOfWeek,
                Date = c.ClassStartingDate,
                FromHour = c.ClassStartingDate.Hour + ":00",
                UntilHour = c.ClassEndingDate.Hour + ":00"

            }).OrderBy(c => c.Date).ToListAsync();

            return classes;
        }
    }
}
