using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToanThangEdu.Areas.Admin.Models
{
    public class TeacherCustomizeModel
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int? YearOfBirth { get; set; }
        public string TeachContent { get; set; }
        public string TeachLevel { get; set; }
        public string TeachArea { get; set; }
        public float? Tuition { get; set; }
        public string MoreContent { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string TeacherType { get; set; }

    }
}