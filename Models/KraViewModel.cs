namespace TelgeProject.Models
{
    public class KraViewModel
    {
        public string KraName { get; set; }
        public DateTime KraStartDate { get; set; }
        public DateTime KraEndDate { get; set; }
        public int Quarter { get; set; }
        public int KeepTarget { get; set; }
        public List<string> KraDescriptions { get; set; }
    }
}