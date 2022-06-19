using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Identity;
using CourseManagement.Model;
using CourseManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly AppDbContext _context;

        public CoursesController(ICourseRepository courseRepository, AppDbContext context)
        {
            _courseRepository = courseRepository;
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();

            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById([FromRoute] int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);

            return Ok(course);
        }
        [HttpGet("students/{id}")]
        public async Task<IActionResult> GetCourseStudents([FromRoute] int id)
        {
            var course = await _courseRepository.GetStudentsOfCourse(id);

            return Ok(course);
        }

        [HttpGet("student/{userId}")]
        public async Task<IActionResult> GetStudentCourses([FromRoute] string userId)
        {
            var courses = await _courseRepository.GetCoursesOfStudentAsync(userId);

            return Ok(courses);
        }


        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseModel courseModel)
        {
            var id = await _courseRepository.AddCourseAsync(courseModel);
            if (id != 0)
            {
                return CreatedAtAction(nameof(GetCourseById), new { id = id, controller = "courses" }, id);
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseModel courseModel, [FromRoute] int id)
        {
            await _courseRepository.UpdateCourseAsync(id, courseModel);


            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCoursePatch([FromBody] JsonPatchDocument courseModel, [FromRoute] int id)
        {
            await _courseRepository.UpdateCoursePatchAsync(id, courseModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            await _courseRepository.DeleteCourseAsync(id);

            return Ok();
        }
        [HttpGet("DBCHECK/{courseName}")]
        public async Task<IActionResult> IsCourseInDb([FromRoute] string courseName)
        {

            return Ok(await _courseRepository.IsCourseInDb(courseName));
            //if( await _courseRepository.IsCourseInDb(courseName))
            //{
            //    return Ok(true);
            //}

            //return Ok(false);
        }


        //[HttpPut("{userId}/{courseId}")]
        //public async Task<IActionResult> AddCourseToStudent([FromRoute] string userId ,[FromRoute] int courseId )
        //{
        //    await _courseRepository.AddCourseStudentToAsync(courseId, userId);
        //    return Ok();
        //}

        [HttpDelete("{userId}/{courseId}")]
        public async Task<IActionResult> DeleteCourseFromStudent([FromRoute] string userId, [FromRoute] int courseId)
        {
            var student =  _context.Students_Course.Where(s => s.UserId == userId && s.CourseId == courseId).First();

            _context.Students_Course.Remove(student);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
