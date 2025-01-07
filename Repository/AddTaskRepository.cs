using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Repository
{
    public class AddTaskRepository : IAddTaskRepository
    {
        private readonly db_telgeprojectContext _context;

        public AddTaskRepository(db_telgeprojectContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<AddTaskViewModel> tasks, int TotalCount)> GetAllTasksAsync(int projectId, string searchTerm, int pageNumber, int pageSize, int userId)
        {
            try
            {
                var users = await _context.TblUsers
                   .Where(u => u.IsDeleted == false && u.Role == 4)
                   .Select(u => new SelectListItem
                   {
                       Value = u.UserId.ToString(),
                       Text = u.FullName
                   })
                   .ToListAsync();

                var taskQuery = from task in _context.TblTasks
                                join user in _context.TblUsers
                                on task.AssignedTo equals user.UserId
                                join project in _context.TblProjects
                                on task.ProjectId equals project.ProjectId
                                join teamLead in _context.TblUsers
                                on project.TeamLeadId equals teamLead.UserId
                                where task.IsDeleted == false && user.IsDeleted == false && user.Role == 4 && task.ProjectId == projectId && task.CreatedBy == userId
                                select new AddTaskViewModel
                                {
                                    TaskId = task.TaskId,
                                    ProjectId = task.ProjectId,
                                    TaskName = task.TaskName,
                                    AssignedTo = user.UserId,
                                    AssignedToName = user.FullName,
                                    AssignedToOptions = users,
                                    Priority = task.Priority,
                                    StartDate = task.StartDate,
                                    EndDate = task.EndDate,
                                    StartDateFormatted = task.StartDate.HasValue ? task.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                                    EndDateFormatted = task.EndDate.HasValue ? task.EndDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                                    TeamLeadName = teamLead.FullName,
                                    EstimatedHours = task.EstimatedHours
                                };


                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    taskQuery = taskQuery.Where(o => o.TaskName.Contains(searchTerm));

                }

                int totalCount = await taskQuery.CountAsync();

                var tasksList = await taskQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (tasksList, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (Enumerable.Empty<AddTaskViewModel>(), 0);
            }
        }

        public Task<IEnumerable<TblTask>> GetAllTasksAsync()
        {
            throw new NotImplementedException();
        }



        //public async Task AddTaskAsync(AddTaskViewModel addTask)
        //{
        //    var addNewTask = new TblTask
        //    {
        //        ProjectId = addTask.ProjectId,
        //        TaskName = addTask.TaskName,
        //        EstimatedHours = addTask.EstimatedHours,
        //        StartDate = addTask.StartDate,
        //        EndDate = addTask.EndDate,
        //        AssignedTo = addTask.AssignedTo,
        //        Status = addTask.Status,
        //        Description = addTask.Description,
        //        CreatedAt = DateTime.Now,
        //        CreatedBy = 2
        //    };
        //    _context.TblTasks.Add(addNewTask);
        //    await _context.SaveChangesAsync();

        //    var taskId = addNewTask.TaskId;

        //    var addSubTask = new TblSubtask
        //    {
        //        SubTaskName = addTask.SubTaskName,
        //        TaskId = taskId,
        //        EstimatedHours = addTask.SubEstimatedHours,
        //        StartDate = addTask.SubEndDate,
        //        EndDate = addTask.SubEndDate,
        //        Priority = addTask.Priority
        //    };
        //    _context.TblSubtasks.Add(addSubTask);
        //    await _context.SaveChangesAsync();
        //}

        public async Task AddTaskAsync(AddTaskViewModel addTask)
        {

            var addNewTask = new TblTask
            {
                ProjectId = addTask.ProjectId,
                TaskName = addTask.TaskName,
                EstimatedHours = addTask.EstimatedHours,
                StartDate = addTask.StartDate,
                EndDate = addTask.EndDate,
                AssignedTo = addTask.AssignedTo,
                Priority = addTask.Priority,
                Description = addTask.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = addTask.CreatedBy
            };

            //Code for save document
            if(addTask.DocumentName != null && addTask.DocumentName.Any())
            {
                var uploadsFolderPath = Path.Combine("wwwroot", "taskDocuments");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath); 
                }

                var uploadedFilePaths = new List<string>();

                foreach (var document in addTask.DocumentName)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(document.FileName);
                    var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                    }

                    uploadedFilePaths.Add(Path.Combine("taskDocuments", uniqueFileName));
                }

                addNewTask.Document = string.Join(";", uploadedFilePaths);
            }

            _context.TblTasks.Add(addNewTask);
            await _context.SaveChangesAsync();

            var createdBy = addNewTask.CreatedBy;

            // Save subtasks
            foreach (var subTask in addTask.SubTasks)
            {
                var addSubTask = new TblSubtask
                {
                    SubTaskName = subTask.SubTaskName,
                    TaskId = addNewTask.TaskId,
                    EstimatedHours = subTask.EstimatedHours,
                    StartDate = subTask.StartDate,
                    AssignedTo = addTask.AssignedTo,
                    EndDate = subTask.EndDate,
                    Priority = subTask.Priority,
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdBy
                };
                _context.TblSubtasks.Add(addSubTask);
            }
            await _context.SaveChangesAsync();
        }

        

        public async Task<AddTaskViewModel> GetTaskByIdAsync(int id)
        {
            var task = await _context.TblTasks
                .FirstOrDefaultAsync(x => x.TaskId == id);

            if(task != null)
            {
                return new AddTaskViewModel
                {
                    TaskId = task.TaskId,
                    ProjectId = task.ProjectId,
                    TaskName = task.TaskName,
                    EstimatedHours = task.EstimatedHours,
                    Description = task.Description,
                    //Status = task.Status,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    AssignedTo = task.AssignedTo,
                    Priority = task.Priority

                };
            }
            return null;
        }
        
        public async Task UpdateTaskAsync(AddTaskViewModel task)
        {
            var existingTask = await _context.TblTasks
           .FirstOrDefaultAsync(c => c.TaskId == task.TaskId);

            if (existingTask == null)
            {
                throw new Exception("Task not found");
            }

            existingTask.ProjectId = task.ProjectId;
            existingTask.TaskName = task.TaskName;
            existingTask.EstimatedHours = task.EstimatedHours;
            existingTask.StartDate = task.StartDate;
            existingTask.EndDate = task.EndDate;
            existingTask.AssignedTo = task.AssignedTo;
            existingTask.Description = task.Description;
            existingTask.Priority = task.Priority;
            existingTask.UpdatedAt = DateTime.Now;
            existingTask.UpdatedBy = task.UpdatedBy;

            var subtasks = await _context.TblSubtasks
                .Where(st => st.TaskId == task.TaskId)
                .ToListAsync();

            foreach (var subtask in subtasks)
            {
                subtask.AssignedTo = task.AssignedTo; 
            }

            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<SubTaskViewModel> subTasks, int TotalCount)> GetAllSubTasksAsync(int taskId, string searchTerm, int pageNumber, int pageSize, int userId)
        {
            try
            {
                var users = await _context.TblUsers
                   .Where(u => u.IsDeleted == false && u.Role == 4)
                   .Select(u => new SelectListItem
                   {
                       Value = u.UserId.ToString(),
                       Text = u.FullName
                   })
                   .ToListAsync();

                var taskQuery = from subTask in _context.TblSubtasks
                                join user in _context.TblUsers
                                on subTask.AssignedTo equals user.UserId
                                join task in _context.TblTasks
                                on subTask.TaskId equals task.TaskId
                                join project in _context.TblProjects
                                on task.ProjectId equals project.ProjectId
                                join teamLead in _context.TblUsers
                                on project.TeamLeadId equals teamLead.UserId
                                where task.IsDeleted == false && user.IsDeleted == false && user.Role == 4 && subTask.TaskId == taskId && subTask.CreatedBy == userId
                                select new SubTaskViewModel
                                {
                                    SubTaskId = subTask.SubTaskId,
                                    FkTaskId = subTask.TaskId,
                                    SubTaskName = subTask.SubTaskName,
                                    TaskName = task.TaskName,
                                    AssignedTo = user.UserId,
                                    AssignedToName = user.FullName,
                                    AssignedToOptions = users,
                                    StartDate = subTask.StartDate,
                                    EndDate = subTask.EndDate,
                                    Priority = subTask.Priority,
                                    StartDateFormatted = subTask.StartDate.HasValue ? task.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                                    EndDateFormatted = subTask.EndDate.HasValue ? task.EndDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                                    TeamLeadName = teamLead.FullName,
                                    EstimatedHours = subTask.EstimatedHours
                                };



                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    taskQuery = taskQuery.Where(o => o.SubTaskName.Contains(searchTerm));

                }

                int totalCount = await taskQuery.CountAsync();

                var tasksList = await taskQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (tasksList, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (Enumerable.Empty<SubTaskViewModel>(), 0);
            }
        }

        public async Task<SubTaskViewModel> GetSubTaskByIdAsync(int id)
        {
            var subtask = await _context.TblSubtasks
                .FirstOrDefaultAsync(x => x.SubTaskId == id);

            if (subtask != null)
            {
                return new SubTaskViewModel
                {
                    SubTaskId = subtask.SubTaskId,
                    FkTaskId = subtask.TaskId,
                    SubTaskName = subtask.SubTaskName,
                    EstimatedHours = subtask.EstimatedHours,
                    StartDate = subtask.StartDate,
                    EndDate = subtask.EndDate,
                    AssignedTo = subtask.AssignedTo,
                    Priority = subtask.Priority
                };
            }
            return null;
        }

        public async Task UpdateSubTaskAsync(SubTaskViewModel subTask)
        {
            var existingSubTask = await _context.TblSubtasks
                .FirstOrDefaultAsync(x => x.SubTaskId == subTask.SubTaskId);

            if (existingSubTask == null)
            {
                throw new Exception("SubTask not found");
            }

            existingSubTask.TaskId = subTask.FkTaskId;
            existingSubTask.SubTaskName = subTask.SubTaskName;
            existingSubTask.EstimatedHours = subTask.EstimatedHours;
            existingSubTask.StartDate = subTask.StartDate;
            existingSubTask.EndDate = subTask.EndDate;
            //existingSubTask.AssignedTo = subTask.AssignedTo;
            existingSubTask.Priority = subTask.Priority;
            existingSubTask.UpdatedAt = DateTime.Now;
            existingSubTask.UpdatedBy = subTask.UpdatedBy;


            await _context.SaveChangesAsync();
        }
    }
}
