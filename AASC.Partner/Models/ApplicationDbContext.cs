using AASC.FW.DataContext;
using AASC.FW.EF6;
using AASC.FW.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,
        IDataContextAsync
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        //: base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public virtual DbSet<FileUpload> FileUploads { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Partner> Partners { get; set; }

        public virtual DbSet<PartnerAgreement> PartnerAgreements { get; set; }

        public virtual DbSet<PartnerGateway> PartnerGateways { get; set; }

        public virtual DbSet<EmployeeRoleInPartner> EmployeeRoleInPartners { get; set; }

        public virtual DbSet<IoTGRoadmap> IoTGRoadmaps { get; set; }

        public virtual DbSet<CLAForm> CLAForms { get; set; }

        public virtual DbSet<PhaseOutPrep> PhaseOutItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(x => x.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(x => x.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(x => new { x.RoleId, x.UserId });            

            modelBuilder.Entity<EmployeeRoleInPartner>().HasKey(x => x.Id);

            modelBuilder.Entity<FileUpload>()
                .HasRequired<ApplicationUser>(x => x.CreatedBy)
                .WithMany(x => x.FileUploads);

            modelBuilder.Entity<Company>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation("Idx_Name", new IndexAnnotation(new[] { new IndexAttribute("Idx_Name") { IsUnique = true } }));

            //modelBuilder.Entity<Department>()
            //    .HasRequired<Company>(x => x.Company)
            //    .WithMany(x => x.Departments)
            //    .WillCascadeOnDelete(false);            
                
            modelBuilder.Entity<Employee>()
                .HasRequired(x => x.ApplicationUser);

            //modelBuilder.Entity<Employee>()
            //    .HasRequired(x => x.Company)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);            

            modelBuilder.Entity<Partner>()
                .HasMany<PartnerAgreement>(x => x.PartmentAgreements)
                .WithRequired(x => x.Partner);
                //.HasForeignKey(x => x.PartnerId);

            //modelBuilder.Entity<EmployeeRoleInPartner>()
            //    .HasRequired<Employee>(x => x.Employee)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<PartnerAgreement>()
            //    .HasRequired<Partner>(x => x.Partner)
            //    .WithMany(x => x.PartmentAgreements)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<PartnerGateway>()
            //    .HasRequired<Partner>(x => x.Partner)
            //    .WithMany(x => x.PartnerGateways)
            //    .WillCascadeOnDelete(false);

        }

        public void SyncObjectState(object entity)
        {
            Entry(entity).State = StateHelper.ConvertState(((IObjectState)entity).ObjectState);
        }


    }
}