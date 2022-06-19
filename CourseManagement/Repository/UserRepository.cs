using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Identity;
using CourseManagement.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<AppUserModel>> GetAllUsers()
        {
            var users = await _context.Users.Select(c => new AppUserModel()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Adress = c.Adress
            }).ToListAsync();

            return users;
        }

        public async Task<List<AppUserModel>> GetAllStudents()
        {
            var users = await _context.Users.Where(s => s.IsProf == false && s.IsAdmin == false).Select(c => new AppUserModel()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Adress = c.Adress
            }).ToListAsync();

            return users;
        }

        public async Task<List<AppUserModel>> GetValidStudentsToAdd(int courseId)
        {
            var studentOfCourse = await _context.Students_Course.Where(s => s.CourseId == courseId).ToListAsync();

            var students = await GetAllStudents();
            var studentsToAdd = new List<AppUserModel>(students);

            foreach (var student in students)
            {
                foreach(var line in studentOfCourse)
                {
                    if(student.Id == line.UserId)
                    {
                        studentsToAdd.Remove(student);
                    }
                }
            }


            return studentsToAdd;
        }

        public async Task<AppUserModel> GetUser(string id)
        {
            var user = await _context.Users.Where(s => s.Id == id).Select(c => new AppUserModel()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Adress = c.Adress
            }).FirstAsync();

            return user;
        }



        public async Task UpdateUser(string id, SignUp appUser)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                user.FirstName = appUser.FirstName;
                user.LastName = appUser.LastName;
                user.Email = appUser.Email;
                user.Adress = appUser.Adress;
                await _context.SaveChangesAsync();
            }

        }

        public async Task<string> DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            var userInCourses = await _context.Students_Course.Where(s => s.UserId == id).ToListAsync();
            var userInClasses = await _context.Students_Class.Where(s => s.UserId == id).ToListAsync();

            if (user != null)
            {
                if (userInCourses.Count > 0)
                {
                    foreach (var record in userInCourses)
                    {
                        _context.Students_Course.Remove(record);
                        await _context.SaveChangesAsync();
                    }
                }
                if (userInClasses.Count > 0)
                {
                    foreach (var record in userInClasses)
                    {
                        _context.Students_Class.Remove(record);
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return user.Id;
        }

        public async Task<string> ChangePassword(string id, ChangePasswordModel changePasswordModel)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
                await _context.SaveChangesAsync();
            }

            return user.Id;
        }

        public async Task<int> AddCourseStudentToAsync(int courseId, string userId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            var student = await _context.Users.FindAsync(userId);

            var student_course = new Student_Course()
            {
                CourseId = courseId,
                Course = course,
                UserId = userId,
                User = student

            };
            await _context.Students_Course.AddAsync(student_course);
            _context.Users.Update(student);
            course.Students.Add(student_course);
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return courseId;
        }

        public async Task<int> DeleteCourseFromStudentAsync(string userId , int courseId)
        {
            var student_course = await _context.Students_Course.FirstOrDefaultAsync(c => c.UserId == userId && c.CourseId == courseId);
            var course_classes = await _context.Classes.Where(c => c.CourseId == courseId).ToListAsync();
            var student_classes = await _context.Students_Class.Where(s => s.UserId == userId).ToListAsync();

            foreach(var courseClass in course_classes)
            {
                foreach(var studentClass in student_classes)
                {
                    if(courseClass.Id == studentClass.ClassId)
                    {
                        _context.Students_Class.Remove(studentClass);
                    }
                }
            }

            _context.Students_Course.Remove(student_course);
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> AddClassesToStudent(string userId, int courseId)
        {
            var student = await _context.Users.FindAsync(userId);
            var classes = await _context.Classes.Where(c => c.CourseId == courseId).ToListAsync();

            foreach (var _class in classes)
            {
                var classToAdd = new Student_Class()
                {
                    UserId = student.Id,
                    ClassId = _class.Id,
                    User = student,
                    _Class = _class

                };
                _context.Students_Class.Add(classToAdd);
            }
            await _context.SaveChangesAsync();
            return 1;

        }
    }
    
    
}
