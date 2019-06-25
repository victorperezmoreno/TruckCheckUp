using System.Data.Entity;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        public DataContext() : base("TruckCheckUpConnection") 
        {
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverComment> DriverComments { get; set; }
        public DbSet<MechanicComment> MechanicComments { get; set; }
        public DbSet<PartCatalog> PartsCatalog { get; set; }
        public DbSet<PartCategory> PartsCategory { get; set; }
        public DbSet<TruckInspection> TruckInspection { get; set; } //PartsReported
        public DbSet<Situation> Situations { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<TruckManufacturer> TruckManufacturer { get; set; }
        public DbSet<TruckModel> TruckModel { get; set; }
        public DbSet<TruckYear> TruckYear { get; set; }

    }
}
