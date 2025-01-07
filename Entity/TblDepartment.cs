using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblDepartment
    {
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
