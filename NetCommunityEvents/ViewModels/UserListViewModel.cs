using System.Collections.Generic;
using NetCommunityEvents.Controllers;

namespace NetCommunityEvents.ViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }

        public int AllUsersLength { get; set; }
    }
}