﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BarrierFreePassengerEntities : DbContext
    {
        public BarrierFreePassengerEntities()
            : base("name=BarrierFreePassengerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<UserExtendBarriers> UserExtendBarriers { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<DispatchStatus> DispatchStatus { get; set; }
    
        public virtual ObjectResult<GetBookingById_Result> GetBookingById(Nullable<int> bookingId)
        {
            var bookingIdParameter = bookingId.HasValue ?
                new ObjectParameter("BookingId", bookingId) :
                new ObjectParameter("BookingId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetBookingById_Result>("GetBookingById", bookingIdParameter);
        }
    }
}