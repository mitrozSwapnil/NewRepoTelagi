using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;
using TelgeProject.Repository;

namespace TelgeProject.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IAddTaskRepository _taskRepository;
        private readonly db_telgeprojectContext _context;
        public ProjectsController(IProjectRepository projectRepository, db_telgeprojectContext context, IAddTaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _context = context;
            _taskRepository = taskRepository;
        }

        public async Task<IActionResult> Projects(string searchTerm = "", int pageNumber = 1, int pageSize = 25)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (userId==0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _projectRepository.GetAllProjectsAsync(searchTerm, pageNumber, pageSize, userId);
            var totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

            var gettotalTasks = await _context.TblTasks.GroupBy(x => x.ProjectId)
                .Select(group => new
                {
                    ProjectId = group.Key,
                    TotalTasks = group.Count()
                }).ToDictionaryAsync(g => g.ProjectId, g => g.TotalTasks);

            var customers = await _context.TblCustomers.Where(x => x.IsDeleted == false).Select(c => new
            {
                Id = c.Id,
                FullName = c.FullName
            }).ToListAsync();

            var projectManagers = await _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 2).Select(u => new
            {
                Id = u.UserId,
                FullName = u.FullName
            }).ToListAsync();

            // Get the counts for each project status
            var ongoingCount = await _context.TblProjects.CountAsync(p => p.Status == "Ongoing");
            var completedCount = await _context.TblProjects.CountAsync(p => p.Status == "Completed");
            var cancelledCount = await _context.TblProjects.CountAsync(p => p.Status == "Cancelled");
            var onHold = await _context.TblProjects.CountAsync(p => p.Status == "OnHold");

            ViewBag.OngoingCount = ongoingCount;
            ViewBag.CompletedCount = completedCount;
            ViewBag.CancelledCount = cancelledCount;
            ViewBag.OnHold = onHold;

            ViewBag.Customers = customers;
            ViewBag.ProjectManagers = projectManagers;

            ViewBag.GetTotalTask = gettotalTasks;

            ViewBag.TotalOrders = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;

            return View(result.projets);
        }

        public async Task<IActionResult> Details(int id, string searchTerm = "", int pageNumber = 1, int pageSize = 25)
        {

            var project = _context.TblProjects
            .AsNoTracking()
            .Where(p => p.ProjectId == id)
            .Select(p => new ProjectViewModel
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                Customer = _context.TblCustomers.Where(x => x.Id == p.CustomerId && x.IsDeleted == false).Select(x => x.FullName).FirstOrDefault(),
                ProjectManager = _context.TblUsers.Where(c => c.UserId == p.ProjectManager && c.IsDeleted == false && c.Role == 2).Select(c => c.FullName).FirstOrDefault(),
                TeamLeadId = _context.TblUsers.Where(c => c.UserId == p.TeamLeadId && c.IsDeleted == false && c.Role == 3).Select(c => c.FullName).FirstOrDefault(),
                EngineerCount = _context.TblTasks.Where(x => x.ProjectId == id && x.IsDeleted == false).Select(x => x.AssignedTo).Distinct().Count(),
                StartDate = Convert.ToString(p.StartDate),
                EstimatedEndDate = Convert.ToString(p.EstimatedEndDate),
                Location = p.Location,
                Budget = p.Budget
            })
            .FirstOrDefault();

            if (project == null)
            {
                return NotFound();
            }


            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _taskRepository.GetAllTasksAsync(id, searchTerm, pageNumber, pageSize, userId);
            project.Tasks = result.tasks;

            var users = await _context.TblUsers.Where(u => u.IsDeleted == false && u.Role == 4).Select(u => new { u.UserId, u.FullName }).ToListAsync();
            ViewBag.Users = new SelectList(users, "UserId", "FullName");

            var taskPendingCount = await _context.TblTasks.CountAsync(p => p.Status == "Pending" && p.ProjectId == id && p.IsDeleted == false);
            ViewBag.taskPCount = taskPendingCount;

            var totalTaskCount = await _context.TblTasks.CountAsync(x => x.ProjectId == id && x.IsDeleted == false);
            ViewBag.totalCount = totalTaskCount;

            ViewBag.TotalOrders = (int)Math.Ceiling((double)result.TotalCount / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;

            return View(project);
        }


        //Update the Assignto Name in using drop down
        [HttpPost]
        public async Task<IActionResult> UpdateAssignedTo(int taskId, int assignedTo, int projectId)
        {
            try
            {
                var task = await _context.TblTasks.FirstOrDefaultAsync(t => t.TaskId == taskId && t.ProjectId == projectId && t.IsDeleted == false);

                if (task != null)
                {
                    task.AssignedTo = assignedTo;

                    var subtasks = await _context.TblSubtasks.Where(st => st.TaskId == taskId && st.IsDeleted == false).ToListAsync();
                    foreach(var subtask in subtasks)
                    {
                        subtask.AssignedTo = assignedTo;
                    }

                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log error
                return Json(new { success = false });
            }
        }




        [HttpGet]
        public async Task <IActionResult> AddProject(string Location,string Customer, string ProjectManager, string ProjectName,string ProjectNumber)
        {
            //Call Store Produre
            string projectNumber;
            using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@newProjectNumber", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                await connection.ExecuteAsync("GenerateProjectNumber", parameters, commandType: CommandType.StoredProcedure);

                projectNumber = parameters.Get<string>("@newProjectNumber");
            }
            //End Store Produre

            var customers = await _context.TblCustomers.Where(x => x.IsDeleted == false).Select(c => new
            {
                Id = c.Id,
                FullName = c.FullName
            }).ToListAsync();

            var projectManagers = await _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 2).Select(u => new
            {
                Id = u.UserId,
                FullName = u.FullName
            }).ToListAsync();

            var teamLead = await _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 3).Select(u => new
            {
                Id = u.UserId,
                Teamlead = u.FullName
            }).ToListAsync();

            var Department = await _context.TblDepartments.Where(x => x.IsDeleted == false).Select(u => new
            {
                Id = u.DepartmentId,
                DeptName = u.DepartmentName
            }).ToListAsync();

            var viewModel = new ProjectViewModel
            {
                Location = Location,
                Customer = Customer,
                ProjectManager = ProjectManager,
                ProjectName = ProjectName,
                //ProjectNumber = ProjectNumber,
                ProjectNumber = projectNumber,

                Customers = new SelectList(customers, "Id", "FullName"),
                ProjectManagers = new SelectList(projectManagers, "Id", "FullName"),
                TeamLeads = new SelectList(teamLead, "Id", "Teamlead"),
                Deparments = new SelectList(Department, "Id", "DeptName")
                
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectViewModel addProject)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            addProject.CreatedBy = userId;

            await _projectRepository.AddProjectAsync(addProject);
            return RedirectToAction(nameof(Projects));
        }






        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }


            project.StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Ongoing", Text = "Ongoing" },
                new SelectListItem { Value = "Completed", Text = "Completed" },
                new SelectListItem { Value = "Cancelled", Text = "Cancelled" },
                new SelectListItem { Value = "OnHold", Text = "OnHold" }
            };

            var customers = await _context.TblCustomers.Where(x => x.IsDeleted == false).Select(c => new
            {
                Id = c.Id,
                FullName = c.FullName
            }).ToListAsync();

            var projectManagers = await _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 2).Select(u => new
            {
                Id = u.UserId,
                FullName = u.FullName
            }).ToListAsync();

            var teamLead = await _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 3).Select(u => new
            {
                Id = u.UserId,
                Teamlead = u.FullName
            }).ToListAsync();

            var department = await _context.TblDepartments.Where(x => x.IsDeleted == false).Select(u => new
            {
                Id = u.DepartmentId,
                DeptName = u.DepartmentName
            }).ToListAsync();


            project.Customers = new SelectList(customers, "Id", "FullName", project.Customer);
            project.ProjectManagers = new SelectList(projectManagers, "Id", "FullName", project.ProjectManager);
            project.TeamLeads = new SelectList(teamLead, "Id", "Teamlead", project.TeamLeadId);
            project.Deparments = new SelectList(department, "Id", "DeptName", project.DeparmentId);

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProject(ProjectViewModel project)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            project.UpdatedBy = userId;

            await _projectRepository.UpdateProjectAsync(project);
            return RedirectToAction(nameof(Projects));
        }

    }
}
