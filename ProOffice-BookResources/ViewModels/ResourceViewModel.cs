using ProOffice_BookResources.Models.ProOffice_Models;

namespace ProOffice_BookResources.ViewModels
{
    public class ResourceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public BookingViewModel? Booking { get; set; }
    }
}
