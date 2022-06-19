using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Identity;
using CourseManagement.Model;
using CourseManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountController(IAccountRepository accountRepository ,IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUp signUpModel)
        {
            var result = await _accountRepository.SignUpAsync(signUpModel);

            if (result.Succeeded)
            {

                return Ok(result.Succeeded);
            }

            return Unauthorized();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] SignIn signInModel)
        {
            var result = await _accountRepository.LogInAsync(signInModel);

            var token = new Token();
            token.JwtToken = result;

            if (string.IsNullOrEmpty(result))
            {

                return Unauthorized();
            }


            return Ok(token);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await _accountRepository.SignOut();


            return Ok();
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllUsers();


            return Ok(users);
        }

    }
}
