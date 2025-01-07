using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblEmployeeReport
    {
        public int Id { get; set; }
        public int? FkDescriptionId { get; set; }
        public int? TargetQuantity { get; set; }
        public string? TargetDeadline { get; set; }
        public int? AchivedQuantity { get; set; }
        public string? AchivedDeadline { get; set; }
        public int? EmpRating { get; set; }
        public string? EmpComment { get; set; }
        public int? ReviewerRating { get; set; }
        public string? ReviewerComment { get; set; }
        public int? FinalRating { get; set; }
        public int? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}