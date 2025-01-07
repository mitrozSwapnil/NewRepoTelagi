using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblReporting
    {
        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int? ReportingPerson { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
