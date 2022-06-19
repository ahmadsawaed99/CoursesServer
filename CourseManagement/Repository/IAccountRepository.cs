using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Identity;
using CourseManagement.Model;
using Microsoft.AspNetCore.Identity;

namespace CourseManagement.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUp signUpModel);
        Task<string> LogInAsync(SignIn signInModel);
        Task SignOut();
        Task<List<AppUserModel>> GetAllUsers();

        Task<string> DeleteUserAsync(int id);
    }
}
