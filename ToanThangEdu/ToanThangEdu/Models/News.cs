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
    
    public partial class News
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string NewsContent { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public Nullable<System.DateTime> NewsDate { get; set; }
        public string ImageURL { get; set; }
        public string CreatorName { get; set; }
    
        public virtual NewsCategory NewsCategory { get; set; }
    }
}
