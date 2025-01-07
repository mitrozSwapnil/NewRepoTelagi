using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblRevisionlog
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? LearningsfromProjectId { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
