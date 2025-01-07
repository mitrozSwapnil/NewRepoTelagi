using Microsoft.AspNetCore.Mvc.Rendering;

namespace TelgeProject.Models
{
    public class AddTaskViewModel
    {
        public int TaskId { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? TeamLeadName { get; set; }
        public int? EstimatedHours { get; set; }
        //public int? ActualHours { get; set; }
        public int? AssignedTo { get; set; }
        public string? AssignedToName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartDateFormatted { get; set; }
        public string EndDateFormatted { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public string Priority { get; set; }
        public IEnumerable<IFormFile> DocumentName { get; set; }



        public SelectList ProjectNames { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public SelectList EngineerName { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public List<SubTaskViewModel> SubTasks { get; set; } = new List<SubTaskViewModel>();

        public List<SelectListItem> AssignedToOptions { get; set; }

    }

    public class SubTaskViewModel
    {
        //Sub Task Table 
        public int SubTaskId { get; set; }
        public int? FkTaskId { get; set; }
        public string? SubTaskName { get; set; }
        public string? TaskName { get; set; }
        public string? SubDescription { get; set; }
        //public string? SubStatus { get; set; }
        public int? EstimatedHours { get; set; }
        //public int? ActualHours { get; set; }
        public int? AssignedTo { get; set; }
        public string? AssignedToName { get; set; }
        public bool? IsDeleted { get; set; }
        public int? IsMainTask { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Priority { get; set; }
        public string? TeamLeadName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public string StartDateFormatted { get; set; }
        public string EndDateFormatted { get; set; }

        public SelectList ProjectNames { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public SelectList EngineerName { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public List<SubTaskViewModel> SubTasks { get; set; } = new List<SubTaskViewModel>();

        public List<SelectListItem> AssignedToOptions { get; set; }
    }
}
