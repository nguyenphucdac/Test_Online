//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test_Online.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Role_Member
    {
        public int Type_Member_Id { get; set; }
        public int Role_Id { get; set; }
        public string Note { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual Type_Member Type_Member { get; set; }
    }
}
