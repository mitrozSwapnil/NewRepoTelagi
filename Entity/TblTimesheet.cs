using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblTimesheet
    {
        public int TimesheetId { get; set; }
        public int? TaskId { get; set; }
        public int? UserId { get; set; }
        public int? HoursLogged { get; set; }
        public DateOnly? LogDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblTask? Task { get; set; }
        public virtual TblUser? UpdatedByNavigation { get; set; }
        public virtual TblUser? User { get; set; }
    }
}
