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
    
    public partial class Topic
    {
        public Topic()
        {
            this.Documents = new HashSet<Document>();
            this.Questions = new HashSet<Question>();
        }
    
        public int Id { get; set; }
        public int Subject_Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Nullable<double> Percen { get; set; }
    
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
