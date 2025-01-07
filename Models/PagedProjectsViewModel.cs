namespace TelgeProject.Models
{
    public class PagedProjectsViewModel
    {
        public IEnumerable<ProjectViewModel> Projects { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
