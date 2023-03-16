using Microsoft.EntityFrameworkCore;
using ProOffice_BookResources.Models.ProOffice_Models;

namespace ProOffice_BookResources.ProOffice_Data
{
    public class ProOfficeContext : DbContext
    {
        public ProOfficeContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=VICTOR-LENOVO;Initial Catalog=ProOfficeBookingResources;Integrated Security=SSPI;TrustServerCertificate=True;");
        }
    }
}
