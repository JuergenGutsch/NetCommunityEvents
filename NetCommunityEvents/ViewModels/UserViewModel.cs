using System;
using NetCommunityEvents.Controllers;
using NetCommunityEvents.Models;

namespace NetCommunityEvents.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsValid { get; set; }

        public Role Role { get; set; }

        public bool Persistent { get; set; }

        public User CreateModel()
        {
            AutoMapper.Mapper.Configuration.CreateMap<UserViewModel, User>();
            return AutoMapper.Mapper.Map<UserViewModel, User>(this);
        }

        public static UserViewModel Create(User user)
        {
            AutoMapper.Mapper.Configuration.CreateMap<User, UserViewModel>();
            return AutoMapper.Mapper.Map<User, UserViewModel>(user);
        }
    }
}