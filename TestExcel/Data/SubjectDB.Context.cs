﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestExcel.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TestExcelEntities : DbContext
    {
        public TestExcelEntities()
            : base("name=TestExcelEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BRANCH> BRANCHes { get; set; }
        public virtual DbSet<BUILDING> BUILDINGs { get; set; }
        public virtual DbSet<COURSE> COURSEs { get; set; }
        public virtual DbSet<DEPARTMENT> DEPARTMENTs { get; set; }
        public virtual DbSet<PROFESSOR> PROFESSORs { get; set; }
        public virtual DbSet<SECTION> SECTIONs { get; set; }
        public virtual DbSet<SUBJECT> SUBJECTs { get; set; }
        public virtual DbSet<USER> USERs { get; set; }
    }
}
