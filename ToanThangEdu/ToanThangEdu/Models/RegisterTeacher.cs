//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToanThangEdu.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RegisterTeacher
    {
        public long Id { get; set; }
        public string Fullname { get; set; }
        public Nullable<int> YearOfBirth { get; set; }
        public Nullable<int> Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string TeachContent { get; set; }
        public string TeachArea { get; set; }
        public Nullable<long> Tuition { get; set; }
        public string Email { get; set; }
        public string MoreContent { get; set; }
    }
}
