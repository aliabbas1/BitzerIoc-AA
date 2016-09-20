using BitzerIoC.Domain.DatabaseContext.ConnectionConstants;
using BitzerIoC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BitzerIoC.Domain.DatabaseContext
{

    /// <summary>
    /// http://ef.readthedocs.io/en/latest/miscellaneous/configuring-dbcontext.html
    /// </summary>
    public class IdentityContext: DbContext
    {
        private readonly IConfigurationRoot _configuration;
        

        #region Basic Config
        /// <summary>
        /// Pass configuration object or null
        /// If null is passed then default connection string is initalized other wise
        /// connection string taken from Configuration object which is passed
        /// </summary>
        /// <param name="configuration">default is null</param>
        public IdentityContext(IConfigurationRoot configuration=null)
        {
            _configuration = configuration;           
        }


        /// <summary>
        /// Low pirority constructor::Additional constructor which accept IdentitContext options
        /// Useful when injecting using DI 
        /// </summary>
        /// <param name="options"></param>
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        /// <summary>
        /// High pirority constructor
        /// </summary>
        /// <param name="optionBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            if (_configuration == null)
               optionBuilder.UseSqlServer(ConnectionConst.BitzerIoCConnectionString);
            else
                optionBuilder.UseSqlServer(_configuration["Data:DefaultConnection:BitzerIoCConnectionString"]);
          
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUserRole>()
                .HasKey(t => new { t.UserId, t.RoleId, t.UserBoundaryId });

            modelBuilder.Entity<AspNetUserRole>()
                .HasOne(u => u.User)
                .WithMany(ur => ur.AspNetUserRoles)
                .HasForeignKey(k => k.UserId);

            modelBuilder.Entity<AspNetUserRole>()
                .HasOne(r => r.Role)
                .WithMany(ur => ur.AspNetUserRoles)
                .HasForeignKey(k => k.RoleId);


            modelBuilder.Entity<AspNetUserRole>()
                .HasOne(b => b.UserBoundary)
                .WithMany(ur => ur.AspNetUserRoles)
                .HasForeignKey(k => k.UserBoundaryId);

        }
        #endregion


        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<UserBoundary> UserBoundaries { get; set; }
        public virtual DbSet<Boundary> Boundaries { get; set; }
        public virtual DbSet<Gateway> Gateways { get; set; }
        public virtual DbSet<UserDevice> UserDevices { get; set; }
        
        public virtual DbSet<Device> Devices { get; set; }

    }
}

