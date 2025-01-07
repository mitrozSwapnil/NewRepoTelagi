using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Controllers
{
    public class CustomersController : Controller
    {

        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Customer(string searchTerm = "", int pageNumber = 1, int pageSize = 25)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _customerRepository.GetAllCustomersAsync(searchTerm, pageNumber, pageSize, userId);
            var totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

            ViewBag.TotalOrders = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;

            return View(result.customers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerViewModel addCustomer)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            addCustomer.CreatedBy = userId;

            await _customerRepository.AddCustomerAsync(addCustomer);
            return RedirectToAction(nameof(Customer));

            //if (ModelState.IsValid)
            //{
                
            //}
            //return View(addCustomer);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCustomer(CustomerViewModel customer)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            customer.UpdatedBy = userId;

            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            try
            {
                await _customerRepository.UpdateCustomerAsync(customer);
                TempData["SuccessMessage"] = "Customer updated successfully!";
                return RedirectToAction("Customer", "Customers");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(customer);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerRepository.DeleteCustomerAsync(id);
                return RedirectToAction("Customer", "Customers");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
