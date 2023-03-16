using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProOffice_BookResources.EmailService;
using ProOffice_BookResources.EmailService.Interface;
using ProOffice_BookResources.Enum;
using ProOffice_BookResources.Models;
using ProOffice_BookResources.Models.ProOffice_Models;
using ProOffice_BookResources.ProOffice_Data;
using ProOffice_BookResources.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProOffice_BookResources.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProOfficeContext _context;
        private readonly IEmailSender _emailSender;

        public HomeController(ProOfficeContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            if (TempData[State.Success] != null)
                ModelState.AddModelError(State.Success, Convert.ToString(TempData[State.Success]) ?? string.Empty);

            if (TempData[State.Error] != null)
                ModelState.AddModelError(State.Error, Convert.ToString(TempData[State.Error]) ?? string.Empty);

            var resources = await _context.Resources.OrderByDescending(x => x.Id).Select(x => new ResourceViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Quantity = x.Quantity,
                Booking = new BookingViewModel() { ResourceId = x.Id }
            }).ToListAsync();

            return View(resources);
        }

        [HttpGet]
        public IActionResult CreateResource()
        {
            if (TempData[State.Success] != null)
                ModelState.AddModelError(State.Success, Convert.ToString(TempData[State.Success]) ?? string.Empty);

            if (TempData[State.Error] != null)
                ModelState.AddModelError(State.Error, Convert.ToString(TempData[State.Error]) ?? string.Empty);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateResource(ResourceViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(State.Error, "Error ModelState");
                    return View(model);
                }

                Resource resource = new Resource()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Quantity = model.Quantity
                };

                await _context.Resources.AddAsync(resource);
                await _context.SaveChangesAsync();

                TempData[State.Success] = "Success created Resource";
                return RedirectToAction(nameof(HomeController.Index));

            }
            catch (Exception)
            {
                ModelState.AddModelError(State.Error, "Error created Resource");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateBooking(int id)
        {

            if (TempData[State.Success] != null)
                ModelState.AddModelError(State.Success, Convert.ToString(TempData[State.Success]) ?? string.Empty);

            if (TempData[State.Error] != null)
                ModelState.AddModelError(State.Error, Convert.ToString(TempData[State.Error]) ?? string.Empty);

            var resource = await _context.Resources.Where(x => x.Id == id).FirstOrDefaultAsync();

            var getResourceIds = _context.Bookings.Where(x => x.ResourceId == resource.Id).ToListAsync();

            return View(getResourceIds);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(State.Error, "Error ModelState");
                    return View(model);
                }

                Resource resource = await _context.Resources.SingleOrDefaultAsync(x => x.Id == model.ResourceId);


                if (model.BookedQunatity > resource.Quantity)
                {
                    ModelState.AddModelError(State.Error, $"Booked Quantity: <strong>{model.BookedQunatity}</strong> is greater than Resource Quantity: <strong>{resource.Quantity}</strong>");
                    return View(model);
                }

                Booking booking = new Booking()
                {
                    ResourceId = model.ResourceId,
                    BookedQunatity = model.BookedQunatity,
                    DateFrom = model.DateFrom,
                    DateTo = model.DateTo,
                    Resource = resource
                };

                _context.Bookings.Add(booking);
                resource.Quantity--;
                await _context.SaveChangesAsync();

                var message = new Message(new string[] 
                                        { "admin@admin.com" },
                                        $"EMAIL SENT TO admin@admin.com FOR CREATED BOOKING WITH ID {booking.Id} ",
                                        $"you have successfully booked your stay in {resource.Name}, со датум од {model.DateFrom.ToShortDateString()} до {model.DateTo.ToShortDateString()} и избравте број на Количина: {model.BookedQunatity}");
                await _emailSender.SendEmailAsync(message);

                TempData[State.Success] = "Success created Resource";
                return RedirectToAction(nameof(HomeController.Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(State.Error, "Error created Booking");
                return RedirectToAction(nameof(HomeController.Index));
            }
        }
    }
}