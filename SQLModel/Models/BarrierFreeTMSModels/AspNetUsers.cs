//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SQLModel.Models.BarrierFreeTMSModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetUsers
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public Nullable<int> GroupId { get; set; }
        public string Contact { get; set; }
        public string WebSite { get; set; }
        public string Note { get; set; }
        public string CreateUserId { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> resetPW { get; set; }
    }
}
