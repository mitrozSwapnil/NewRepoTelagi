using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class AttachDocument
    {
        public int Id { get; set; }
        public int? FkId { get; set; }
        public string? DocumentName { get; set; }
        public int? Flag { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
