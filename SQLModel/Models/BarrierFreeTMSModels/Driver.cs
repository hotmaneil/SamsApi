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
    
    public partial class Driver
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Driver()
        {
            this.TaxiDriver = new HashSet<TaxiDriver>();
        }
    
        public int DriverId { get; set; }
        public string IdentityNo { get; set; }
        public string DriverName { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> Sex { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<int> BloodTypeId { get; set; }
        public string PAddress { get; set; }
        public string LAddress { get; set; }
        public string HomeTelNo { get; set; }
        public Nullable<int> LicenseTypeId { get; set; }
        public string LicenseNo { get; set; }
        public Nullable<System.DateTime> LicenseTakeDate { get; set; }
        public Nullable<System.DateTime> LicenseFialDate { get; set; }
        public string RegistrationNo { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public string MobileNo { get; set; }
        public Nullable<int> TaxiCompanyGroupId { get; set; }
        public string UserId { get; set; }
        public string PersonalPhoto { get; set; }
        public string IdentityPhoto { get; set; }
        public string LicensePhoto { get; set; }
        public string RegistrationPhoto { get; set; }
        public Nullable<int> EvaluationNumber { get; set; }
        public Nullable<double> Rank { get; set; }
        public string MVPNo { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> drvState { get; set; }
        public Nullable<System.DateTime> openDate { get; set; }
        public Nullable<System.DateTime> quitDate { get; set; }
        public Nullable<System.DateTime> SuspensionDate { get; set; }
        public Nullable<bool> NeverHire { get; set; }
        public Nullable<System.DateTime> RejoinDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxiDriver> TaxiDriver { get; set; }
    }
}