using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Controllers
{

    public class KRAController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IAddTaskRepository _taskRepository;
        private readonly IKRAService _kraService;
        private readonly db_telgeprojectContext _context;

        public KRAController(IProjectRepository projectRepository, db_telgeprojectContext context, IKRAService kRAService, IAddTaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _context = context;
            _taskRepository = taskRepository;
            _kraService = kRAService;
        }
        public async Task<IActionResult> KRAIndex()
        {
            var project = await _context.TblProjects.Where(x => x.IsDeleted == false).Select(u => new
            {
                Id = u.ProjectId,
                ProjectName = u.ProjectName
            }).ToListAsync();

            var assignedTo = await _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 4).Select(u => new
            {
                Id = u.UserId,
                EngineerName = u.FullName
            }).ToListAsync();

            var viewModel = new AddTaskViewModel
            {
                ProjectNames = new SelectList(project, "Id", "ProjectName"),
                EngineerName = new SelectList(assignedTo, "Id", "EngineerName")
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateKRA([FromBody] KraViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            try
            {
                await _kraService.GenerateKRAAsync(model);
                return Ok(new { success = true, message = "KRA generated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        public async Task<IActionResult> KraReport(string searchTerm = " ", int pageNumber = 1, int pageSize = 10)
        {
            var kraWithDescriptions = await _context.Kras
             .Select(k => new KrareportViewModel
             {
                 KraId = k.Id,
                 KraName = k.KraName,
                 Quarter = k.Quarter,
                 TotalEmp = 20,
                 Kra_Submited = 10,
                 Kra_Pending = 10,

             }).ToListAsync();
            return View(kraWithDescriptions);
        }
        public IActionResult KraReportEmployee()
        {
            return View();
        }

        public IActionResult KraReportReview()
        {
            return View();
        }









    }
}