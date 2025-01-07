﻿using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblSubtask
    {
        public TblSubtask()
        {
            TblNotifications = new HashSet<TblNotification>();
        }

        public int SubTaskId { get; set; }
        public int? TaskId { get; set; }
        public string? SubTaskName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }
        public int? AssignedTo { get; set; }
        public bool? IsDeleted { get; set; }
        public int? IsMainTask { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Priority { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual TblUser? AssignedToNavigation { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblTask? Task { get; set; }
        public virtual TblUser? UpdatedByNavigation { get; set; }
        public virtual ICollection<TblNotification> TblNotifications { get; set; }
    }
}
