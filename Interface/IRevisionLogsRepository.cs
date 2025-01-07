using TelgeProject.Models;

namespace TelgeProject.Interface
{
    public interface IRevisionLogsRepository
    {
        Task<(IEnumerable<RevisionLogsViewModel> revision, int TotalCount)> GetAllRevisionsAsync(int Id, string searchTerm, int pageNumber, int pageSize, int userId);
        Task AddRevisionLogsAsync(RevisionLogsViewModel revisionLogs);
    }
}
