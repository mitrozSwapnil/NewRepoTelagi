using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;
using TelgeProject.Repository;

namespace TelgeProject.Controllers
{
    public class TaskSubTasksController : Controller
    {
        private readonly IAddTaskRepository _taskRepository;
        private readonly db_telgeprojectContext _context;

        public TaskSubTasksController(IAddTaskRepository taskRepository, db_telgeprojectContext context)
        {
            _taskRepository = taskRepository;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> AddTask(int projectId)
        {
            var getprojectId = _context.TblProjects.FirstOrDefault(p => p.ProjectId == projectId);

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
                EngineerName = new SelectList(assignedTo, "Id", "EngineerName"),

                 ProjectId = projectId,
                ProjectName = getprojectId?.ProjectName
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(AddTaskViewModel addTask)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            addTask.CreatedBy = userId;


            if (addTask.DocumentName != null && addTask.DocumentName.Any())
            {
                foreach (var file in addTask.DocumentName)
                {
                    Console.WriteLine($"Received file: {file.FileName}");
                }
            }
            else
            {
                Console.WriteLine("No files received.");
            }

            await _taskRepository.AddTaskAsync(addTask);

            return Json(new { success = true, redirectUrl = Url.Action("Details", "Projects", new { id = addTask.ProjectId }) });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTask(int Id)
        {
            var gettask = await _taskRepository.GetTaskByIdAsync(Id);
            if(gettask == null)
            {
                return NotFound();
            }

            var assignTo = await _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 4).Select(u => new
            {
                Id = u.UserId,
                AssignTo = u.FullName
            }).ToListAsync();
            gettask.EngineerName = new SelectList(assignTo, "Id", "AssignTo", gettask.AssignedTo);

            return View(gettask);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask(AddTaskViewModel task)
        {

            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            task.UpdatedBy = userId;

            await _taskRepository.UpdateTaskAsync(task);
            return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
        }

        [HttpGet]
        public async Task<IActionResult> SubTask(int id, string searchTerm = "", int pageNumber = 1, int pageSize = 25)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _taskRepository.GetAllSubTasksAsync(id, searchTerm, pageNumber, pageSize, userId);
            var totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

            var taskName = _context.TblTasks.Where(x => x.TaskId == id && x.IsDeleted == false).Select(x => x.TaskName).FirstOrDefault();

            ViewBag.TotalOrders = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TaskName = taskName;


            return View(result.subTasks);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSubTask(int Id)
        {
            var getsubtask = await _taskRepository.GetSubTaskByIdAsync(Id);
            if (getsubtask == null)
            {
                return NotFound();
            }

            var assignTo = await _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 4).Select(u => new
            {
                Id = u.UserId,
                AssignTo = u.FullName
            }).ToListAsync();
            getsubtask.EngineerName = new SelectList(assignTo, "Id", "AssignTo", getsubtask.AssignedTo);

            return View(getsubtask);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubTask(SubTaskViewModel subtask)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            subtask.UpdatedBy = userId;

            await _taskRepository.UpdateSubTaskAsync(subtask);
            return RedirectToAction("SubTask", "TaskSubTasks", new { id = subtask.FkTaskId });
        }
    }
}
