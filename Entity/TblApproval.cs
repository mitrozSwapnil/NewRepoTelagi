using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblApproval
    {
        public int ApprovalId { get; set; }
        public int? TaskId { get; set; }
        public int? RequestedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public string? Status { get; set; }
        public int? ExtraHoursRequested { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual TblUser? ApprovedByNavigation { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? RequestedByNavigation { get; set; }
        public virtual TblTask? Task { get; set; }
        public virtual TblUser? UpdatedByNavigation { get; set; }
    }
}
