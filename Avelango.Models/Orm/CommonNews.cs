//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Avelango.Models.Orm
{
    using System;
    using System.Collections.Generic;
    
    public partial class CommonNews
    {
        public int ID { get; set; }
        public System.Guid PublicKey { get; set; }
        public string Html { get; set; }
        public System.DateTime Created { get; set; }
        public string Lang { get; set; }
        public Nullable<System.Guid> Theme { get; set; }
    }
}
