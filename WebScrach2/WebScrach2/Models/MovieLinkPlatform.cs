//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebScrach2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MovieLinkPlatform
    {
        public int MLPID { get; set; }
        public Nullable<int> MovieID { get; set; }
        public Nullable<int> PlatformID { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual Movy Movy { get; set; }
        public virtual Plateform Plateform { get; set; }
    }
}
