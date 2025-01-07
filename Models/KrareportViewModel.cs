namespace TelgeProject.Models
{
    public class KrareportViewModel
    {
        public int KraId { get; set; }
        public string KraName { get; set; }
        public int? Quarter { get; set; }
        public int? TotalEmp { get; set; }
        public int? Kra_Submited { get; set; }
        public int? Kra_Pending { get; set; }
    }
}