using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly db_telgeprojectContext _context;

        public ProjectRepository(db_telgeprojectContext context)
        {
            _context = context;
        }

        

        public async Task<(IEnumerable<ProjectViewModel> projets, int TotalCount)> GetAllProjectsAsync(string searchTerm, int pageNumber, int pageSize, int userId)
        {
            try
            {
                var query = from project in _context.TblProjects
                            join customer in _context.TblCustomers
                            on project.CustomerId equals customer.Id
                            join user in _context.TblUsers
                            on project.ProjectManager equals user.UserId
                            where project.IsDeleted == false && project.CreatedBy == userId
                            select new ProjectViewModel
                            {
                                ProjectId = project.ProjectId,
                                ProjectName = project.ProjectName,
                                Customer = customer.FullName,
                                ProjectNumber = project.ProjectNumber,
                                ProjectManager = user.FullName,
                                StartDate = project.StartDate.ToString(),
                                EstimatedEndDate =project.EstimatedEndDate.ToString(),
                            };

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(o => o.Customer.Contains(searchTerm) ||
                                             o.ProjectName.Contains(searchTerm) ||
                                             o.ProjectManager.Contains(searchTerm) ||
                                             o.ProjectNumber.Contains(searchTerm));
                }

                int totalCount = await query.CountAsync();

                var projectsList = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (projectsList, totalCount);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");

                return (Enumerable.Empty<ProjectViewModel>(), 0);
            }
        }

        //public async Task<IEnumerable<TblProject>> GetAllProjectsAsync()
        //{
        //    return await _context.TblProjects.Where(u => u.IsDeleted == false).ToListAsync();
        //}



        public async Task AddProjectAsync(ProjectViewModel project)
        {
            var addProject = new TblProject
            {
                ProjectName = project.ProjectName,
                ProjectNumber = project.ProjectNumber,
                Address = project.Address,
                Location = project.Location,
                Country = project.Country,
                State = project.State,
                Contractor = project.Contractor,
                ProjectAwardedFrom = project.ProjectAwardedFrom,
                PrecasterFabricator = project.PrecasterFabricator,
                StructuralDesigner = project.StructuralDesigner,
                Architecture = project.Architecture,
                ModellingDetailingFirm = project.ModellingDetailingFirm,
                EstimatedHours = project.EstimatedHours,
                Budget = project.Budget,
                HourlyRate = project.HourlyRate,
                StartDate = string.IsNullOrEmpty(project.StartDate) ? null : DateOnly.Parse(project.StartDate),
                EstimatedEndDate = string.IsNullOrEmpty(project.EstimatedEndDate) ? null : DateOnly.Parse(project.EstimatedEndDate),
                Status = project.Status,
                ProjectManager = string.IsNullOrEmpty(project.ProjectManager) ? null : Convert.ToInt32(project.ProjectManager),
                CustomerId = string.IsNullOrEmpty(project.Customer) ? null : Convert.ToInt32(project.Customer),
                TeamLeadId = string.IsNullOrEmpty(project.TeamLeadId) ? null : Convert.ToInt32(project.TeamLeadId),
                DeparmentId = string.IsNullOrEmpty(project.DeparmentId) ? null : Convert.ToInt32(project.DeparmentId),
                Discription = project.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = project.CreatedBy
            };


            if (project.Document != null && project.Document.Any())
            {
                var uploadsFolderPath = Path.Combine("wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var uploadedFilePaths = new List<string>();

                foreach (var document in project.Document)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(document.FileName);
                    var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                    }

                    uploadedFilePaths.Add(Path.Combine("uploads", uniqueFileName));
                }

                addProject.Document = string.Join(";", uploadedFilePaths); 
            }

            _context.TblProjects.Add(addProject);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjectViewModel> GetProjectByIdAsync(int projectId)
        {
            var project = await _context.TblProjects.FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project == null)
            {
                return null;
            }

            return new ProjectViewModel
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectNumber = project.ProjectNumber,
                Address = project.Address,
                Location = project.Location,
                Country = project.Country,
                State = project.State,
                StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Ongoing", Text = "Ongoing" },
                    new SelectListItem { Value = "Completed", Text = "Completed" },
                    new SelectListItem { Value = "Cancelled", Text = "Cancelled" },
                    new SelectListItem { Value = "OnHold", Text = "OnHold" }
                },
                Contractor = project.Contractor,
                ProjectAwardedFrom = project.ProjectAwardedFrom,
                PrecasterFabricator = project.PrecasterFabricator,
                StructuralDesigner = project.StructuralDesigner,
                Architecture = project.Architecture,
                ModellingDetailingFirm = project.ModellingDetailingFirm,
                EstimatedHours = project.EstimatedHours,
                Budget = project.Budget,
                HourlyRate = project.HourlyRate,
                StartDate = project.StartDate?.ToString("dd-MM-yyyy"),
                EstimatedEndDate = project.EstimatedEndDate?.ToString("dd-MM-yyyy"),
                //NewStartDate = Convert.ToDateTime(project.StartDate),
                //EndDate = Convert.ToDateTime(project.EstimatedEndDate),
                Status = project.Status,
                ProjectManager = project.ProjectManager?.ToString(),
                Customer = project.CustomerId.ToString(),
                TeamLeadId = project.TeamLeadId.ToString(),
                DeparmentId = project.DeparmentId.ToString(),
                Description = project.Discription,
                ExistingDocuments = project.Document?.Split(';').ToList() ?? new List<string>()
            };
        }

        public async Task UpdateProjectAsync(ProjectViewModel project)
        {
            var existingProject = await _context.TblProjects.FirstOrDefaultAsync(p => p.ProjectId == project.ProjectId);

            if (existingProject != null)
            {
                existingProject.ProjectName = project.ProjectName;
                existingProject.ProjectNumber = project.ProjectNumber;
                existingProject.Address = project.Address;
                existingProject.Location = project.Location;
                existingProject.Country = project.Country;
                existingProject.State = project.State;
                existingProject.Contractor = project.Contractor;
                existingProject.ProjectAwardedFrom = project.ProjectAwardedFrom;
                existingProject.PrecasterFabricator = project.PrecasterFabricator;
                existingProject.StructuralDesigner = project.StructuralDesigner;
                existingProject.Architecture = project.Architecture;
                existingProject.ModellingDetailingFirm = project.ModellingDetailingFirm;
                existingProject.EstimatedHours = project.EstimatedHours;
                existingProject.Budget = project.Budget;
                existingProject.HourlyRate = project.HourlyRate;
                existingProject.StartDate = DateOnly.Parse(project.StartDate);
                existingProject.EstimatedEndDate = DateOnly.Parse(project.EstimatedEndDate);
                existingProject.Status = project.Status;
                existingProject.ProjectManager = string.IsNullOrEmpty(project.ProjectManager) ? null : Convert.ToInt32(project.ProjectManager);
                existingProject.CustomerId = Convert.ToInt32(project.Customer);
                existingProject.TeamLeadId = Convert.ToInt32(project.TeamLeadId);
                existingProject.DeparmentId = Convert.ToInt32(project.DeparmentId);
                existingProject.Discription = project.Description;
                existingProject.UpdatedAt = DateTime.Now;
                existingProject.UpdatedBy = project.UpdatedBy;

                if (project.DocumentsToRemove != null && project.DocumentsToRemove.Any())
                {
                    foreach (var fileToRemove in project.DocumentsToRemove)
                    {
                        var fullPath = Path.Combine("wwwroot", fileToRemove);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    var remainingDocuments = existingProject.Document?.Split(';')
                        .Where(doc => !project.DocumentsToRemove.Contains(doc)).ToList();

                    existingProject.Document = remainingDocuments != null ? string.Join(";", remainingDocuments) : null;
                }


                if (project.Document != null && project.Document.Any())
                {
                    var uploadsFolder = Path.Combine("wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var newDocumentPaths = new List<string>();
                    foreach (var document in project.Document)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(document.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await document.CopyToAsync(stream);
                        }

                        newDocumentPaths.Add(Path.Combine("uploads", uniqueFileName));
                    }

                    // Ensure the new document paths are correctly added to the existing ones
                    var allDocuments = (existingProject.Document?.Split(';') ?? new string[] { }).Concat(newDocumentPaths);
                    existingProject.Document = string.Join(";", allDocuments);
                }


                await _context.SaveChangesAsync();
            }
        }


        //public async Task<(IEnumerable<ProjectViewModel> Projects, int TotalCount)> GetAllProjectsAsync(int pageNumber, int pageSize)
        //{
        //    var query = _context.TblProjects
        //        .Where(p => p.IsDeleted == false)
        //        .Select(p => new ProjectViewModel
        //        {
        //            ProjectId = p.ProjectId,
        //            ProjectNumber = p.ProjectNumber,
        //            ProjectName = p.ProjectName,
        //            Address = p.Address,
        //            Location = p.Location,
        //            Country = p.Country,
        //            State = p.State,
        //            Contractor = p.Contractor,
        //            PrecasterFabricator = p.PrecasterFabricator,
        //            StructuralDesigner = p.StructuralDesigner,
        //            Architecture = p.Architecture,
        //            ModellingDetailingFirm = p.ModellingDetailingFirm,
        //            EstimatedHours = p.EstimatedHours,
        //            Budget = p.Budget,
        //            HourlyRate = p.HourlyRate,
        //            // StartDate = p.StartDate,
        //            // EstimatedEndDate = p.EstimatedEndDate,
        //            Status = p.Status,
        //            Description = p.Discription
        //        });

        //    int totalCount = await query.CountAsync();
        //    var projects = await query
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    return (projects, totalCount);
        //}

        //public async Task<ProjectViewModel> GetProjectByIdAsync(int id)
        //{
        //    return await _context.TblProjects
        //        .Where(p => p.ProjectId == id && p.IsDeleted==false)
        //        .Select(p => new ProjectViewModel
        //        {
        //            ProjectId = p.ProjectId,
        //            ProjectNumber = p.ProjectNumber,
        //            ProjectName = p.ProjectName,
        //            Address = p.Address,
        //            Location = p.Location,
        //            Country = p.Country,
        //            State = p.State,
        //            Contractor = p.Contractor,
        //            PrecasterFabricator = p.PrecasterFabricator,
        //            StructuralDesigner = p.StructuralDesigner,
        //            Architecture = p.Architecture,
        //            ModellingDetailingFirm = p.ModellingDetailingFirm,
        //            EstimatedHours = p.EstimatedHours,
        //            Budget = p.Budget,
        //            HourlyRate = p.HourlyRate,
        //            Status = p.Status,
        //            Description = p.Discription
        //        })
        //        .FirstOrDefaultAsync();
        //}

        //public async Task AddProjectAsync(TblProject project)
        //{
        //    await _context.TblProjects.AddAsync(project);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateProjectAsync(TblProject project)
        //{
        //    _context.TblProjects.Update(project);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteProjectAsync(int id)
        //{
        //    var project = await _context.TblProjects.FirstOrDefaultAsync(p => p.ProjectId == id);
        //    if (project != null)
        //    {
        //        project.IsDeleted = true;
        //        _context.TblProjects.Update(project);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
