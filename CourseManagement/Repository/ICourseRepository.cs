using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace CourseManagement.Repository
{
    public interface ICourseRepository
    {
        Task<List<CourseModel>> GetAllCoursesAsync();
        Task<CourseModel> GetCourseByIdAsync(int id);
        Task<int> AddCourseAsync(CourseModel courseModel);
        Task UpdateCourseAsync(int id, CourseModel courseModel);
        Task UpdateCoursePatchAsync(int id, JsonPatchDocument courseModel);
        Task<int> DeleteCourseAsync(int id);
        Task<List<CourseModel>> GetCoursesOfStudentAsync(string userId);
        Task<bool> IsCourseInDb(string courseName);
        Task<List<AppUserModel>> GetStudentsOfCourse(int courseId);
    }
}
