﻿using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblCustomer
    {
        public TblCustomer()
        {
            TblProjects = new HashSet<TblProject>();
        }

        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }
        public string? Gstno { get; set; }
        public string? BussinessName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual ICollection<TblProject> TblProjects { get; set; }
    }
}
