//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SQLModel.Models.BarrierFreePassengerModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class DispatchStatus
    {
        public System.Guid OrderId { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public Nullable<System.DateTime> OrderTime { get; set; }
        public Nullable<System.DateTime> DispatchTime { get; set; }
        public Nullable<bool> IsCancel { get; set; }
        public Nullable<int> CancelFrom { get; set; }
        public Nullable<int> FlowStatus { get; set; }
        public Nullable<System.DateTime> GetonTime { get; set; }
        public Nullable<System.DateTime> GetoffTime { get; set; }
        public Nullable<System.DateTime> ArrivalTime { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> PromiseMinute { get; set; }
    }
}
