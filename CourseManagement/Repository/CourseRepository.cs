using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseManagement.Data;
using CourseManagement.Identity;
using CourseManagement.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public CourseRepository(AppDbContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseModel>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses.Select(c => new CourseModel()
            {
                Id = c.Id,
                Name = c.Name,
                EndingDate = c.EndingDate,
                StartingDate = c.StartingDate,
                //DaysAndHoursOfCourses = c.DaysAndHoursOfCourses


            }).OrderBy(c => c.Name).ToListAsync();

            return courses;
        }

        public async Task<CourseModel> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            return _mapper.Map<CourseModel>(course);
        }

        public async Task<List<CourseModel>> GetCoursesOfStudentAsync(string userId)
        {
            var Studens_course = await _context.Students_Course.Where(s => s.UserId == userId).ToListAsync();

            var courses = new List<CourseModel>();

            foreach(var _course in Studens_course)
            {
                var course = await _context.Courses.FindAsync(_course.CourseId);

                var courseToAdd = _mapper.Map<CourseModel>(course);

                courses.Add(courseToAdd);
            }

            return courses;
        }
        public async Task<List<AppUserModel>> GetStudentsOfCourse(int courseId)
        {
            var Course_Students = await _context.Students_Course.Where(c => c.CourseId == courseId).ToListAsync();

            var studentsOfCourse = new List<AppUserModel>();

            foreach(var line in Course_Students)
            {
                var student = await _context.Users.FindAsync(line.UserId);

                studentsOfCourse.Add(_mapper.Map<AppUserModel>(student));
            }

            return studentsOfCourse;

        }


        public async Task<int> AddCourseAsync(CourseModel courseModel)
        {

            if(!(false) && IsValidDates(courseModel))
            {
                var course = new Courses()
                {
                    Name = courseModel.Name,
                    StartingDate = courseModel.StartingDate,
                    EndingDate = courseModel.EndingDate,
                    DaysAndHoursOfCourses = courseModel.DaysAndHoursOfCourses

                };

                AddClassesToCourse(course);
                await _context.Courses.AddAsync(course);
                await _context.SaveChangesAsync();

                return course.Id;
            }
            return 0;

            
        }

        public async Task UpdateCourseAsync(int id, CourseModel courseModel)
        {

            if (IsValidDates(courseModel))
            {
                var course = new Courses()
                {
                    Id = id,
                    Name = courseModel.Name,
                    StartingDate = courseModel.StartingDate,
                    EndingDate = courseModel.EndingDate,
                    DaysAndHoursOfCourses = courseModel.DaysAndHoursOfCourses
                };


                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCoursePatchAsync(int id ,JsonPatchDocument courseModel)
        {
            var course = await _context.Courses.FindAsync(id);
            
            if(course != null)
            {
                courseModel.ApplyTo(course);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<int> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            var courseInStudents_Course = await _context.Students_Course.Where(c => c.CourseId == id).ToListAsync();
            var courseClasses = await _context.Classes.Where(c => c.CourseId == id).ToListAsync();

            if (courseInStudents_Course != null)
            {
                foreach (var c in courseInStudents_Course)
                {
                    _context.Students_Course.Remove(c);
                }
            }
            if (courseClasses != null)
            {
                foreach (var c in courseClasses)
                {
                    _context.Classes.Remove(c);
                }
            }

            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }

            return course.Id;
        }


        public async Task<bool>  IsCourseInDb(string courseName)
        {
            var courseInDb = await _context.Courses.Where(c=> c.Name == courseName).SingleOrDefaultAsync();

            if (courseInDb != null)
            {
                return true;
            }
            return false;
        }

        private bool IsValidDates(CourseModel course)
        {
            if (course.StartingDate > course.EndingDate)
            {
                return false;
            }
            if (course.EndingDate < DateTime.Now)
            {
                return false;
            }

            return true;
        }

        private void AddClassesToCourse(Courses course)
        {
            var startDate = course.StartingDate;
            var endDate = course.EndingDate;
            var classes = new List<Class>();
            var numberOfDays = endDate.DayOfYear - startDate.DayOfYear;

            for(int i=0; i < numberOfDays; i++)
            {
                foreach(var day in course.DaysAndHoursOfCourses)
                {
                    startDate = course.StartingDate;
                    if (startDate.AddDays(i).DayOfWeek == day.Day)
                    {
                        var classToAdd = new Class()
                        {
                            ClassStartingDate = new DateTime(startDate.AddDays(i).Year, startDate.AddDays(i).Month, startDate.AddDays(i).Day, day.StartClassHour, 00, 00),
                            ClassEndingDate = new DateTime(startDate.AddDays(i).Year, startDate.AddDays(i).Month, startDate.AddDays(i).Day, day.EndClassHour, 00, 00),
                            CourseId = course.Id,
                            Course = course
                        };

                        classes.Add(classToAdd);
                    }
                }
            }

            course.Classes = classes;
        }


        
    }
}
