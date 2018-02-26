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
    
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.Chats = new HashSet<Chats>();
            this.Chats1 = new HashSet<Chats>();
            this.Notifications = new HashSet<Notifications>();
            this.Notifications1 = new HashSet<Notifications>();
            this.Portfolios = new HashSet<Portfolios>();
            this.RialtoAction = new HashSet<RialtoAction>();
            this.RialtoUserAssets = new HashSet<RialtoUserAssets>();
            this.RialtoUsersStates = new HashSet<RialtoUsersStates>();
            this.TaskBids = new HashSet<TaskBids>();
            this.TaskOffers = new HashSet<TaskOffers>();
            this.TaskPreWorkers = new HashSet<TaskPreWorkers>();
            this.TempUserData = new HashSet<TempUserData>();
        }
    
        public int ID { get; set; }
        public Nullable<System.Guid> PublicKey { get; set; }
        public bool IsCompany { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Phone1 { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Skype { get; set; }
        public string Ballance { get; set; }
        public string Valuta { get; set; }
        public System.DateTime AccountCreated { get; set; }
        public Nullable<System.DateTime> LastLogIn { get; set; }
        public string UserLogoPath { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string Birthday { get; set; }
        public Nullable<double> Rating { get; set; }
        public string Honors { get; set; }
        public string SubscribeToGroups { get; set; }
        public bool IsEnabled { get; set; }
        public bool ReceiveNews { get; set; }
        public string Lang { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public Nullable<bool> IsModerator { get; set; }
        public string UserLogoPathMax { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> EmailDelivery { get; set; }
        public Nullable<bool> SmsDelivery { get; set; }
        public Nullable<bool> PushUpDelivery { get; set; }
        public bool IsModerChecked { get; set; }
        public string PlaceName { get; set; }
        public Nullable<double> PlaceLat { get; set; }
        public Nullable<double> PlaceLng { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chats> Chats { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chats> Chats1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notifications> Notifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notifications> Notifications1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolios> Portfolios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RialtoAction> RialtoAction { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RialtoUserAssets> RialtoUserAssets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RialtoUsersStates> RialtoUsersStates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskBids> TaskBids { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskOffers> TaskOffers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskPreWorkers> TaskPreWorkers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TempUserData> TempUserData { get; set; }
    }
}
