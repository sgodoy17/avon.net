//using System.Data.Entity;
//using System.Data.Entity.ModelConfiguration.Conventions;
//using System.Diagnostics;
//using Microsoft.AspNet.Identity.EntityFramework;
//using IdentiGo.Domain.Security;
//using System;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.Migrations.History;
//using IdentiGo.Domain.Entity;
//using IdentiGo.Domain.Entity.General;
//using IdentiGo.Domain.Entity.IdentiGo;
//using IdentiGo.Domain.Entity.IdentiGo.Page;
//using IdentiGo.Domain.Entity.Master;
//using MySql.Data.Entity;
//using IdentiGo.Domain.Entity.CIFIN;

//namespace IdentiGo.Data
//{
//    [DbConfigurationType(typeof(MySqlEFConfiguration))]
//    public class MainContext : IdentityDbContext<User, Role, Guid, UserLogin, UserRole, UserClaim>, IDisposable
//    {
//        #region General

//        public DbSet<Company> Company { get; set; }

//        public DbSet<Config> Config { get; set; }

//        public DbSet<Question> Question { get; set; }

//        public DbSet<QuestionOption> QuestionOption { get; set; }

//        public DbSet<Page> Page { get; set; }

//        public DbSet<UserAffiliation> UserAffiliation { get; set; }

//        public DbSet<UserValidation> UserValidation { get; set; }

//        public DbSet<BlackList> BlackList { get; set; }


//        #endregion

//        #region Master

//        public DbSet<Country> Country { get; set; }

//        public DbSet<Department> Department { get; set; }

//        public DbSet<City> City { get; set; }

//        public DbSet<VoteSite> VoteSite { get; set; }

//        public DbSet<AffiliationType> AffiliationType { get; set; }

//        public DbSet<SecretaryTransit> SecretaryTransit { get; set; }

//        public DbSet<Unit> Unit { get; set; }

//        public DbSet<Zone> Zone { get; set; }

//        public DbSet<Division> Division { get; set; }

//        public DbSet<Quota> Quota { get; set; }

//        public DbSet<RiskLevel> RiskLevel { get; set; }

//        #endregion

//        #region Pages

//        public DbSet<Queue> Queue { get; set; }

//        public DbSet<Attorney> Attorney { get; set; }

//        public DbSet<Controllership> Controllership { get; set; }

//        public DbSet<Fosyga> Fosyga { get; set; }

//        public DbSet<Policeman> Policeman { get; set; }

//        public DbSet<Registrar> Registrar { get; set; }

//        public DbSet<Ruaf> Ruaf { get; set; }

//        public DbSet<RuafAssistance> RuafAssistance { get; set; }

//        public DbSet<RuafCompensation> RuafCompensation { get; set; }

//        public DbSet<RuafHealth> RuafHealth { get; set; }

//        public DbSet<RuafPension> RuafPension { get; set; }

//        public DbSet<RuafRisks> RuafRisks { get; set; }

//        public DbSet<RuafSeverance> RuafSeverance { get; set; }
        
//        public DbSet<Runt> Runt { get; set; }

//        public DbSet<Sena> Sena { get; set; }

//        public DbSet<Simit> Simit { get; set; }

//        public DbSet<Sisben> Sisben { get; set; }

//        #endregion

//        public MainContext()
//            : base("name=DBContext")
//        {
//            Database.SetInitializer<MainContext>(new CreateDatabaseIfNotExists<MainContext>());
//            Database.Log = msg => Debug.WriteLine(msg);
//        }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            //Use UPPERCASE for Tables Name
//            modelBuilder.Types().Configure(c => c.ToTable(c.ClrType.Name.ToUpper()));

//            //Use UPPERCASE for Columns Name
//            modelBuilder.Properties().Configure(c => c.HasColumnName(c.ClrPropertyInfo.Name.ToUpper()));

//            //Disable Pluralization for Tables Name
//            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

//            modelBuilder.Entity<User>().ToTable("USER");
//            modelBuilder.Entity<User>().Property(h => h.UserName).HasMaxLength(200).IsRequired();
//            modelBuilder.Entity<User>().Property(h => h.Email).HasMaxLength(200).IsRequired();

//            modelBuilder.Entity<UserClaim>().ToTable("USERCLAIM");

//            modelBuilder.Entity<UserLogin>().ToTable("USERLOGIN");

//            modelBuilder.Entity<UserRole>().ToTable("USERROLE");

//            modelBuilder.Entity<Role>().ToTable("ROLE").HasKey(t => t.Id).Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
//            modelBuilder.Entity<Role>().Property(h => h.Name).HasMaxLength(200).IsRequired();

//        }

//        public static MainContext Create()
//        {
//            return new MainContext();
//        }

//        public virtual void Commit()
//        {
//            base.SaveChanges();
//        }

//    }
//}

