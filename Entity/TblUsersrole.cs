using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblUsersrole
    {
        public TblUsersrole()
        {
            TblUsers = new HashSet<TblUser>();
        }

        public int RoleId { get; set; }
        public string? Role { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual ICollection<TblUser> TblUsers { get; set; }
    }
}
