using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProOffice_BookResources.Models.ProOffice_Models
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int BookedQunatity { get; set; }
        public int ResourceId { get; set; } // FK to Resource
        public virtual Resource Resource { get; set; }
    }
}
