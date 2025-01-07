using System;
using System.Collections.Generic;

namespace TelgeProject.Entity
{
    public partial class TblDescription
    {
        public int Id { get; set; }
        public int? KraIdfk { get; set; }
        public string? Description { get; set; }

        public virtual Kra? KraIdfkNavigation { get; set; }
    }
}