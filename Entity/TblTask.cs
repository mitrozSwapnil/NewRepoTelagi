using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblTask
    {
        public TblTask()
        {
            TblApprovals = new HashSet<TblApproval>();
            TblNotifications = new HashSet<TblNotification>();
            TblSubtasks = new HashSet<TblSubtask>();
            TblTimesheets = new HashSet<TblTimesheet>();
        }

        public int TaskId { get; set; }
        public int? ProjectId { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }
        public int? AssignedTo { get; set; }
        public string? Document { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Priority { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual TblUser? AssignedToNavigation { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblProject? Project { get; set; }
        public virtual TblUser? UpdatedByNavigation { get; set; }
        public virtual ICollection<TblApproval> TblApprovals { get; set; }
        public virtual ICollection<TblNotification> TblNotifications { get; set; }
        public virtual ICollection<TblSubtask> TblSubtasks { get; set; }
        public virtual ICollection<TblTimesheet> TblTimesheets { get; set; }
    }
}
