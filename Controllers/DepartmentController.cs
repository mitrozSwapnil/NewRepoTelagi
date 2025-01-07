using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelgeProject.Interface;
using TelgeProject.Models;
using TelgeProject.Repository;

namespace TelgeProject.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _depRepository;

        public DepartmentController(IDepartmentRepository depRepository)
        {
            _depRepository = depRepository;
        }

        public async Task<IActionResult> DepartmentList(string searchTerm = "", int pageNumber = 1, int pageSize = 25)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _depRepository.GetAllDeparmentsAsync(searchTerm, pageNumber, pageSize, userId);
            var totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

            ViewBag.TotalOrders = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;

            return View(result.deparments);
        }


        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromForm] DeparmentViewModel addDepartment)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            addDepartment.CreatedBy = userId;

            await _depRepository.AddDeparmentAsync(addDepartment);
            //return RedirectToAction(nameof(DepartmentList));
            return Json(new { success = true });

        }

        [HttpGet]
        public async Task<IActionResult> EditDepartment(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid department ID");
            }

            var department = await _depRepository.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound("Department not found");
            }

            return Json(department); // Return data as JSON for modal
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(DeparmentViewModel department)
        {

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            department.UpdatedBy = userId;

            try
            {
                await _depRepository.UpdateDepartmentAsync(department);
                return Json(new { success = true, message = "Department updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            try
            {
                await _depRepository.DeleteDepartmentAsync(id);
                return RedirectToAction("DepartmentList", "Department");
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                return Json(new { success = false, message = $"Database Update Error: {innerException}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"General Error: {ex.Message}" });
            }
        }

    }
}
