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
    
    public partial class CommonAdvertising
    {
        public int ID { get; set; }
        public System.Guid PublicKey { get; set; }
        public string CompanyName { get; set; }
        public int CompanyRate { get; set; }
        public System.DateTime Expired { get; set; }
        public string ImagePath { get; set; }
        public string FirstData { get; set; }
        public string SecondData { get; set; }
        public string Icon { get; set; }
    }
}