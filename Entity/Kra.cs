using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class Kra
    {
        public Kra()
        {
            TblDescriptions = new HashSet<TblDescription>();
        }

        public int Id { get; set; }
        public string? KraName { get; set; }
        public int? Quarter { get; set; }
        public int? IsTarget { get; set; }
        public int? FkUserId { get; set; }
        public DateTime? KraStartDate { get; set; }
        public DateTime? KraEndDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public int? IsDelete { get; set; }

        public virtual ICollection<TblDescription> TblDescriptions { get; set; }
    }
}