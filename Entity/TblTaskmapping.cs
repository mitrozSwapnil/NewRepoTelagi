using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblTaskmapping
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public int? UserId { get; set; }
        public int? AssignBy { get; set; }
        public DateOnly? DateOfassign { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
