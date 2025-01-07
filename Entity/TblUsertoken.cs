using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblUsertoken
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? IsDelete { get; set; }

        public virtual TblUser? User { get; set; }
    }
}