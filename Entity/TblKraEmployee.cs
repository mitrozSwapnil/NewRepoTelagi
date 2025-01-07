using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblKraEmployee
    {
        public int Id { get; set; }
        public int? FkKraId { get; set; }
        public int? FkEmployeeId { get; set; }
        public int? KraGeneratedpersonId { get; set; }
        public DateTime? Date { get; set; }
        public int? KraIsSubmit { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}