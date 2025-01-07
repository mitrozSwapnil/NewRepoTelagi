using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblProject
    {
        public TblProject()
        {
            TblMilestones = new HashSet<TblMilestone>();
            TblNotifications = new HashSet<TblNotification>();
            TblTasks = new HashSet<TblTask>();
        }

        public int ProjectId { get; set; }
        public string? ProjectNumber { get; set; }
        public string? ProjectName { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Contractor { get; set; }
        public string? ProjectAwardedFrom { get; set; }
        public string? PrecasterFabricator { get; set; }
        public string? StructuralDesigner { get; set; }
        public string? Architecture { get; set; }
        public string? ModellingDetailingFirm { get; set; }
        public int? EstimatedHours { get; set; }
        public decimal? Budget { get; set; }
        public decimal? HourlyRate { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EstimatedEndDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Status { get; set; }
        public string? Discription { get; set; }
        public string? Document { get; set; }
        public int? ProjectManager { get; set; }
        public int? CustomerId { get; set; }
        public int? DeparmentId { get; set; }
        public int? TeamLeadId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblCustomer? Customer { get; set; }
        public virtual TblUser? ProjectManagerNavigation { get; set; }
        public virtual TblUser? UpdatedByNavigation { get; set; }
        public virtual ICollection<TblMilestone> TblMilestones { get; set; }
        public virtual ICollection<TblNotification> TblNotifications { get; set; }
        public virtual ICollection<TblTask> TblTasks { get; set; }
    }
}
