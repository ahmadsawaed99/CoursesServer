using System;
using AutoMapper;
using CourseManagement.Data;
using CourseManagement.Identity;
using CourseManagement.Model;

namespace CourseManagement.Helpers
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<Courses, CourseModel>().ReverseMap();
            CreateMap<AppUserModel, AppUser>().ReverseMap();
        }
    }
}
