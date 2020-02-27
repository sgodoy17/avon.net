using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentiGo.Domain.Security;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Domain.Entity.Log;
using IdentiGo.Domain.Entity.AVON;

namespace IdentiGo.Data
{
    public class MainContext : IdentityDbContext<User, Role, Guid, UserLogin, UserRole, UserClaim>, IDisposable
    {
        #region General

        public DbSet<Company> Company { get; set; }

        public DbSet<Config> Config { get; set; }

        public DbSet<UserAffiliation> UserAffiliation { get; set; }

        public DbSet<Nomination> UserValidation { get; set; }

        public DbSet<NominationResponse> NominationResponse { get; set; }

        public DbSet<NominationHistoric> NominationHistoric { get; set; }

        public DbSet<BlackList> BlackList { get; set; }

        #endregion

        #region Master

        public DbSet<Country> Country { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<City> City { get; set; }

        public DbSet<VoteSite> VoteSite { get; set; }

        public DbSet<AffiliationType> AffiliationType { get; set; }

        public DbSet<SecretaryTransit> SecretaryTransit { get; set; }

        public DbSet<Unit> Unit { get; set; }

        public DbSet<Zone> Zone { get; set; }

        public DbSet<Division> Division { get; set; }

        public DbSet<Quota> Quota { get; set; }

        public DbSet<RiskLevel> RiskLevel { get; set; }

        #endregion

        public DbSet<LogIVR> LogIVR { get; set; }

        public DbSet<LogSMS> LogSMS { get; set; }

        #region Log

        #endregion

        public MainContext()
            : base("name=DBContext")
        {
            Database.SetInitializer<MainContext>(null);
            Database.Log = msg => Debug.WriteLine(msg);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Use UPPERCASE for Tables Name
            modelBuilder.Types().Configure(c => c.ToTable(c.ClrType.Name.ToUpper()));

            //Use UPPERCASE for Columns Name
            modelBuilder.Properties().Configure(c => c.HasColumnName(c.ClrPropertyInfo.Name.ToUpper()));

            //Disable Pluralization for Tables Name
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>().ToTable("USER");
            modelBuilder.Entity<User>().Property(h => h.UserName).HasMaxLength(200).IsRequired();
            modelBuilder.Entity<User>().Property(h => h.Email).HasMaxLength(200).IsRequired();

            modelBuilder.Entity<UserClaim>().ToTable("USERCLAIM");

            modelBuilder.Entity<UserLogin>().ToTable("USERLOGIN");

            modelBuilder.Entity<UserRole>().ToTable("USERROLE");

            modelBuilder.Entity<CashPayment>().ToTable("CASHPAYMENT");

            modelBuilder.Entity<Role>().ToTable("ROLE").HasKey(t => t.Id).Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Role>().Property(h => h.Name).HasMaxLength(200).IsRequired();
        }

        public static MainContext Create()
        {
            return new MainContext();
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

    }
}

