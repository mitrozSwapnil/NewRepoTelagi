namespace TelgeProject.Models
{
    public class RevisionLogsViewModel
    {
        public int RevisionId { get; set; }
        public int? ProjectId { get; set; }
        public string? SubId { get; set; }
        public string? ScopeOfWork { get; set; }
        public string? Reason { get; set; }
        //public string? DocumentName { get; set; }
        public IEnumerable<IFormFile> DocumentName { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? ManHours { get; set; }
        public string? RevisionBy { get; set; }
        public string? ExecutedBy { get; set; }
        public string? Remark { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
