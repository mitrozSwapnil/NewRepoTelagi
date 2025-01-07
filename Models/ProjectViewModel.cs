using Microsoft.AspNetCore.Mvc.Rendering;

namespace TelgeProject.Models
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }
       
        public string Address { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Contractor { get; set; }
        public string ProjectAwardedFrom { get; set; }
        public string PrecasterFabricator { get; set; }
        public string StructuralDesigner { get; set; }
        public string Architecture { get; set; }
        public string ModellingDetailingFirm { get; set; }
        public int? EstimatedHours { get; set; }
        public decimal? Budget { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public decimal? HourlyRate { get; set; }
        public string StartDate { get; set; }
        public string? EstimatedEndDate { get; set; }

        public DateTime NewStartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public int EngineerCount { get; set; }
        public string ProjectManager { get; set; }
        public string Customer { get; set; }
        public string TeamLeadId { get; set; }
        public string DeparmentId { get; set; }
        public string Description { get; set; }
        public IEnumerable<IFormFile> Document { get; set; }
        public List<string> ExistingDocuments { get; set; }
        public List<string> DocumentsToRemove { get; set; }

        //public IFormFile Document { get; set; }

        public List<documentlistmodel> dlist {  get; set; }

        public SelectList ProjectManagers { get; set; }
        public SelectList Customers { get; set; }
        public SelectList TeamLeads { get; set; }
        public SelectList Deparments { get; set; }

        public IEnumerable<AddTaskViewModel> Tasks { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }

        public IEnumerable<SelectListItem> StatusOptions { get; set; }

    }

    public class documentlistmodel
    {
        public string filename { get; set; }
        public string url { get; set; }
    }
   
}
