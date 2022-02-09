using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.ViewModels
{
    public class RoleUpdateViewModel 
    {
        [Display(Name = "角色Id")]
        public string Id { set; get; }

        [Required(ErrorMessage = "角色名称不能为空")]
        [Display(Name = "角色名称")]
        public string Name { set; get; }

        public List<UserRoleViewModel> Users { set; get; }
    }
}
