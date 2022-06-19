using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : Controller
    {
        private readonly IClassRepository _classRepository;
        private readonly AppDbContext _context;

        public ClassController(IClassRepository classRepository, AppDbContext context)
        {
            _classRepository = classRepository;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentsOfTheClass(int id)
        {
            var studentsOfTheClass = await _classRepository.GetStudentsOfClassAsync(id);

            return Ok(studentsOfTheClass);
        }

        [HttpGet("course/{id}")]
        public async Task<IActionResult> GetCourseClasses(int id)
        {
            var classes = await _classRepository.GetCourseClassesAsync(id);

            return Ok(classes);
        }
        [HttpGet("course-student/{id}")]
        public async Task<IActionResult> GetStudentCourseClasses(int id)
        {
            var classes = await _classRepository.GetStudentCourseClassesAsync(id);

            return Ok(classes);
        }

    }
}
