using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CourseManagement.Data;
using CourseManagement.Identity;
using CourseManagement.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CourseManagement.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _cfg;
        private readonly AppDbContext _context;

        public AccountRepository(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager, IConfiguration cfg , AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cfg = cfg;
            _context = context;
        }

        public async Task<IdentityResult> SignUpAsync(SignUp signUpModel)
        {
            var user = new AppUser()
            {
                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                UserName = signUpModel.Email,
                Email = signUpModel.Email,
                Adress = signUpModel.Adress,
                IsAdmin = signUpModel.IsAdmin,
                IsProf = signUpModel.IsProf

            };


            if (user.IsAdmin && user.IsProf)
            {
                await _userManager.CreateAsync(user, signUpModel.Password);
                await _userManager.AddToRoleAsync(user, "Prof");
                return await _userManager.AddToRoleAsync(user, "Admin");
            }

            if (user.IsProf && !user.IsAdmin)
            {
                await _userManager.CreateAsync(user, signUpModel.Password);
                return await _userManager.AddToRoleAsync(user, "Prof");
            }
            
            return await _userManager.CreateAsync(user,signUpModel.Password);
        }


        public async Task<string> LogInAsync(SignIn signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
            
            
            if (!result.Succeeded)
            {
                return null;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
            };

            var authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_cfg["JWT:Secret"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256Signature)
                );
           
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
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
        public async Task<string> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return user.Id;
        }
    }
}
