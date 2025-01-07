using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TelgeProject.Interface;
using TelgeProject.Models;
using TelgeProject.Repository;

namespace TelgeProject.Controllers
{
    public class RevisionLogsController : Controller
    {
        private readonly IRevisionLogsRepository _revisionlogsRepository;

        public RevisionLogsController(IRevisionLogsRepository revisionlogsRepository)
        {
            _revisionlogsRepository = revisionlogsRepository;
        }

        public async Task<IActionResult> RevisionLogs(int id, string searchTerm = "", int pageNumber = 1, int pageSize = 25)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _revisionlogsRepository.GetAllRevisionsAsync(id, searchTerm, pageNumber, pageSize, userId);
            var totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

            ViewBag.TotalOrders = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.projectId = id;

            return View(result.revision);
        }

        [HttpGet]
        public  IActionResult AddRevisionLogs(int id)
        {
            if(id == 0)
            {
                return BadRequest("Invalid ProjectId.");
            }

            var model = new RevisionLogsViewModel
            {
                ProjectId = id 
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRevisionLogs(RevisionLogsViewModel addRevision, int id)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            addRevision.CreatedBy = userId;


            addRevision.ProjectId = id;

            if (addRevision.DocumentName != null && addRevision.DocumentName.Any())
            {
                foreach (var file in addRevision.DocumentName)
                {
                    Console.WriteLine($"Received file: {file.FileName}");
                }
            }
            else
            {
                Console.WriteLine("No files received.");
            }

            await _revisionlogsRepository.AddRevisionLogsAsync(addRevision);
                return RedirectToAction("RevisionLogs", "RevisionLogs", new { id = addRevision.ProjectId });
        }
    }
}
