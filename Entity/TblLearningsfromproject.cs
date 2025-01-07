using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblLearningsfromproject
    {
        public int Id { get; set; }
        public int? Name { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
