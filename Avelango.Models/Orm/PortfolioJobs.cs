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
    
    public partial class PortfolioJobs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PortfolioJobs()
        {
            this.PortfolioJobImages = new HashSet<PortfolioJobImages>();
        }
    
        public int ID { get; set; }
        public Nullable<System.Guid> PublicKey { get; set; }
        public Nullable<int> BelongsToPortfolio { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PortfolioJobImages> PortfolioJobImages { get; set; }
        public virtual Portfolios Portfolios { get; set; }
    }
}
