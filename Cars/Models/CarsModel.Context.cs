﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cars.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Car_Rent_SystemEntities2 : DbContext
    {
        public Car_Rent_SystemEntities2()
            : base("name=Car_Rent_SystemEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Client_dbo> Client_dbo { get; set; }
        public virtual DbSet<Company_dbo> Company_dbo { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<ViewCarOrderModel> Tb_Order { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
    }
}
