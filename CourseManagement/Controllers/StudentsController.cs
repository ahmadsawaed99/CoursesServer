using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Identity;
using CourseManagement.Model;
using CourseManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public StudentsController(UserManager<AppUser> userManager, AppDbContext context , IUserRepository userRepository , IAccountRepository accountRepository)
        {
            _userManager = userManager;
            _context = context;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetAllStudents()
        {
            var users = await _userRepository.GetAllStudents();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            var user = await _userRepository.GetUser(id);

            return Ok(user);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetValidStudentsToAdd([FromRoute] int courseId)
        {
            var students = await _userRepository.GetValidStudentsToAdd(courseId);

            return Ok(students);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] string id)
        {
            await _userRepository.DeleteUserAsync(id);

            return Ok(id);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateStudent([FromBody] SignUp signUpModel)
        {
            signUpModel.IsAdmin = false;
            signUpModel.IsProf = false;

            var result = await _accountRepository.SignUpAsync(signUpModel);

            if (result.Succeeded)
            {

                return Ok(result.Succeeded);
            }

            return BadRequest();
        }

        [HttpPost("{userId}/{courseId}")]
        public async Task<IActionResult> AddCourseToStudent([FromRoute] string userId,[FromRoute] int courseId)
        {
            await _userRepository.AddCourseStudentToAsync(courseId, userId);
            await _userRepository.AddClassesToStudent(userId, courseId);

            return Ok();
        }

        [HttpDelete("{userId}/{courseId}")]
        public async Task<IActionResult> DeleteCourseFromStudent([FromRoute] string userId, [FromRoute] int courseId)
        {
            await _userRepository.DeleteCourseFromStudentAsync( userId,courseId);

            return Ok();
        }

    }
}
