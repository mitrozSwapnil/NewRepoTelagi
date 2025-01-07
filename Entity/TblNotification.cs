using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblNotification
    {
        public int NotificationId { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public int? SubTaskId { get; set; }
        public string? NotificationType { get; set; }
        public string? NotificationFor { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblProject? Project { get; set; }
        public virtual TblSubtask? SubTask { get; set; }
        public virtual TblTask? Task { get; set; }
        public virtual TblUser? User { get; set; }
    }
}
