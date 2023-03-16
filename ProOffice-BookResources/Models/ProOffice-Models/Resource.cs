using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProOffice_BookResources.Models.ProOffice_Models
{
    [Table("Resources")]
    public class Resource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

    }
}
