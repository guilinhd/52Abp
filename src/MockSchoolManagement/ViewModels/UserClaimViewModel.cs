using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.ViewModels
{
    public class UserClaimViewModel
    {
        public string Id { set; get; }

        public List<UserRoleViewModel> Claims { set; get; }
    }
}
