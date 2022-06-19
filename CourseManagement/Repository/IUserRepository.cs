using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Identity;
using CourseManagement.Model;

namespace CourseManagement.Repository
{
    public interface IUserRepository
    {
        Task<List<AppUserModel>> GetAllUsers();
        Task<List<AppUserModel>> GetAllStudents();
        Task<AppUserModel> GetUser(string id);
        Task UpdateUser(string id, SignUp appUser);
        Task<string> DeleteUserAsync(string id);
        Task<string> ChangePassword(string id, ChangePasswordModel changePasswordModel);
        Task<int> AddCourseStudentToAsync(int courseId, string userId);
        Task<int> AddClassesToStudent(string userId, int courseId);
        Task<int> DeleteCourseFromStudentAsync(string userId, int courseId);
        Task<List<AppUserModel>> GetValidStudentsToAdd(int courseId);
    }
}
