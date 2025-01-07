using Microsoft.AspNetCore.Mvc.Rendering;

namespace TelgeProject.Models
{
    public class AddProjectViewModel
    {
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
        public int EstimatedHours { get; set; }
        public decimal Budget { get; set; }
        public decimal HourlyRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public string Status { get; set; }
        public string ProjectManager { get; set; }
        public string CustomerId { get; set; }
        public string Description { get; set; }
        public string Document { get; set; }

        public IEnumerable<SelectListItem> customerlist { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> statuslist { get; set; } = new List<SelectListItem>();
    }
}
