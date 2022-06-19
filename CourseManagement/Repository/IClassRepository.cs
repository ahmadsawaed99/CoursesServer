using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Model;

namespace CourseManagement.Repository
{
    public interface IClassRepository
    {
        Task<List<StudentsAttendance>> GetStudentsOfClassAsync(int classId);

        Task<List<ClassModel>> GetCourseClassesAsync(int courseId);

        Task<List<ClassModel>> GetStudentCourseClassesAsync(int courseId);
    }
}
