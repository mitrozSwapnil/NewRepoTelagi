using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblApprovalApprovedByNavigations = new HashSet<TblApproval>();
            TblApprovalCreatedByNavigations = new HashSet<TblApproval>();
            TblApprovalRequestedByNavigations = new HashSet<TblApproval>();
            TblApprovalUpdatedByNavigations = new HashSet<TblApproval>();
            TblMilestoneCreatedByNavigations = new HashSet<TblMilestone>();
            TblMilestoneUpdatedByNavigations = new HashSet<TblMilestone>();
            TblNotificationCreatedByNavigations = new HashSet<TblNotification>();
            TblNotificationUsers = new HashSet<TblNotification>();
            TblProjectCreatedByNavigations = new HashSet<TblProject>();
            TblProjectProjectManagerNavigations = new HashSet<TblProject>();
            TblProjectUpdatedByNavigations = new HashSet<TblProject>();
            TblSubtaskAssignedToNavigations = new HashSet<TblSubtask>();
            TblSubtaskCreatedByNavigations = new HashSet<TblSubtask>();
            TblSubtaskUpdatedByNavigations = new HashSet<TblSubtask>();
            TblTaskAssignedToNavigations = new HashSet<TblTask>();
            TblTaskCreatedByNavigations = new HashSet<TblTask>();
            TblTaskUpdatedByNavigations = new HashSet<TblTask>();
            TblTimesheetCreatedByNavigations = new HashSet<TblTimesheet>();
            TblTimesheetUpdatedByNavigations = new HashSet<TblTimesheet>();
            TblTimesheetUsers = new HashSet<TblTimesheet>();
            TblUsertokens = new HashSet<TblUsertoken>();
        }

        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Contact { get; set; }
        public string? Profile { get; set; }
        public int? Role { get; set; }
        public string? Responsibilities { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? Status { get; set; }

        public virtual TblUsersrole? RoleNavigation { get; set; }
        public virtual ICollection<TblApproval> TblApprovalApprovedByNavigations { get; set; }
        public virtual ICollection<TblApproval> TblApprovalCreatedByNavigations { get; set; }
        public virtual ICollection<TblApproval> TblApprovalRequestedByNavigations { get; set; }
        public virtual ICollection<TblApproval> TblApprovalUpdatedByNavigations { get; set; }
        public virtual ICollection<TblMilestone> TblMilestoneCreatedByNavigations { get; set; }
        public virtual ICollection<TblMilestone> TblMilestoneUpdatedByNavigations { get; set; }
        public virtual ICollection<TblNotification> TblNotificationCreatedByNavigations { get; set; }
        public virtual ICollection<TblNotification> TblNotificationUsers { get; set; }
        public virtual ICollection<TblProject> TblProjectCreatedByNavigations { get; set; }
        public virtual ICollection<TblProject> TblProjectProjectManagerNavigations { get; set; }
        public virtual ICollection<TblProject> TblProjectUpdatedByNavigations { get; set; }
        public virtual ICollection<TblSubtask> TblSubtaskAssignedToNavigations { get; set; }
        public virtual ICollection<TblSubtask> TblSubtaskCreatedByNavigations { get; set; }
        public virtual ICollection<TblSubtask> TblSubtaskUpdatedByNavigations { get; set; }
        public virtual ICollection<TblTask> TblTaskAssignedToNavigations { get; set; }
        public virtual ICollection<TblTask> TblTaskCreatedByNavigations { get; set; }
        public virtual ICollection<TblTask> TblTaskUpdatedByNavigations { get; set; }
        public virtual ICollection<TblTimesheet> TblTimesheetCreatedByNavigations { get; set; }
        public virtual ICollection<TblTimesheet> TblTimesheetUpdatedByNavigations { get; set; }
        public virtual ICollection<TblTimesheet> TblTimesheetUsers { get; set; }
        public virtual ICollection<TblUsertoken> TblUsertokens { get; set; }
    }
}