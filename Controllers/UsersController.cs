using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;
using TelgeProject.Repository;

namespace TelgeProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly db_telgeprojectContext _context;

        public UsersController(IUserRepository userRepository, db_telgeprojectContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }
        public async Task<IActionResult> Index(string searchTerm = "", int pageNumber = 1, int pageSize = 25)
        {
            var result = await _userRepository.GetAllUsersAsync(searchTerm,pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

            var engcount = _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 4).ToList().Count();
            var teamledercount = _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 3).ToList().Count();
            var projmancount = _context.TblUsers.Where(x => x.IsDeleted == false && x.Role == 2).ToList().Count();

            ViewBag.TotalOrders = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.ProjectManager = projmancount;
            ViewBag.TeamLeader = teamledercount;
            ViewBag.Engineer = engcount;

            return View(result.users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var roles = _context.TblUsersroles.Select(x=> new SelectListItem
            {
                Text=x.Role,
                Value=x.RoleId.ToString()
            }).ToList();


            var repotingPerson = _context.TblUsers.Where(x => x.Role == 2 ||  x.Role == 3 || x.Role == 1).Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.UserId.ToString()
            }).ToList();

            var Responsibilities = new List<SelectListItem>
            {
                new SelectListItem { Text = "manage", Value = "manage" },
                new SelectListItem { Text = "edit", Value = "edit" },
            };

            var viewModel = new addUserViewModel
            {
               
                Responsibilitieslist= Responsibilities,
                RoleList = roles,
                ReportingPersonList = repotingPerson
            };

            return View(viewModel);
           
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(addUserViewModel model)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);

            var data = new TblUser
            {
                FullName = model.FullName,
                Email = model.Email,
                Contact = model.Contact,
                PasswordHash = hashedPassword,
                Role = model.Role,
                Responsibilities = model.Responsibilities,
                CreatedAt = DateTime.Now,
                CreatedBy = 1
            };

            _context.TblUsers.Add(data);
            _context.SaveChanges();

            var fkid = data.UserId;

            var reportingdata = new TblReporting
            {
                FkUserId = fkid,
                ReportingPerson = model.userId,
                IsDeleted = false
            };

            _context.TblReportings.Add(reportingdata);
            _context.SaveChanges();
               
            TempData["SuccessMessage"] = "User has been added successfully!";
            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {

            var user = _context.TblUsers.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            var reportingData = _context.TblReportings.FirstOrDefault(x => x.FkUserId == user.UserId);
            int? reportingPersonId = reportingData?.ReportingPerson;

            var roles = _context.TblUsersroles.Select(x => new SelectListItem
            {
                Text = x.Role,
                Value = x.RoleId.ToString()
            }).ToList();


            var repotingPerson = _context.TblUsers.Where(x => x.Role == 2 || x.Role == 3).Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.UserId.ToString()
            }).ToList();

            var viewModel = new addUserViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Contact = user.Contact,
                PasswordHash = string.Empty,
                Role = user.Role,
                ReportingPerson = reportingPersonId ?? 0,
                Responsibilities = user.Responsibilities,
                RoleList = roles,
                ReportingPersonList = repotingPerson,
                Responsibilitieslist = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Manage", Value = "manage" },
                    new SelectListItem { Text = "Edit", Value = "edit" }
                }
            };

            return View(viewModel);
        }
        
       

        [HttpPost]
        public IActionResult EditUser(int id, addUserViewModel model)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);

            if (!ModelState.IsValid)
            {
                  model.RoleList = _context.TblUsersroles.Select(x => new SelectListItem { Text = x.Role, Value = x.RoleId.ToString() }).ToList();
                  model.ReportingPersonList = _context.TblReportings.Select(x => new SelectListItem { Text = Convert.ToString(x.FkUserId), Value = x.ReportingPerson.ToString() }).ToList();
                  model.Responsibilitieslist = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Manage", Value = "manage" },
                        new SelectListItem { Text = "Edit", Value = "edit" }
                    }; 

                return View(model);
            }

            var user = _context.TblUsers.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Contact = model.Contact;
            user.PasswordHash = hashedPassword;
            user.Role = model.Role;
            user.Responsibilities = model.Responsibilities;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = 1;

            var reporting = _context.TblReportings.FirstOrDefault(x => x.FkUserId == id);
            reporting.ReportingPerson = model.ReportingPerson;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "User details updated successfully!";
            return RedirectToAction("Index"); 
        }


        public IActionResult DeleteUser(int id)
        {
            var user = _context.TblUsers.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = 1;


            _context.SaveChanges();


            return RedirectToAction("Index");
        }

    }

}
