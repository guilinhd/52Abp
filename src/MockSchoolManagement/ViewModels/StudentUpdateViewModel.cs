using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.ViewModels
{
    public class StudentUpdateViewModel : StudentCreateViewModel
    {
        public int Id { set; get; }

        public string ExistingPhotoPath { set; get; }
    }
}
