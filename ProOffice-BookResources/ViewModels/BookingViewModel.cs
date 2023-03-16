using ProOffice_BookResources.Models.ProOffice_Models;

namespace ProOffice_BookResources.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int BookedQunatity { get; set; }
        public int ResourceId { get; set; } // FK to Resource
        //public Resource Resource { get; set; }
    }
}
