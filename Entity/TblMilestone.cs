using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblMilestone
    {
        public int MilestoneId { get; set; }
        public int? ProjectId { get; set; }
        public string? MilestoneName { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblProject? Project { get; set; }
        public virtual TblUser? UpdatedByNavigation { get; set; }
    }
}
