using TelgeProject.Entity;

namespace TelgeProject.Interface
{
    public interface IKRARepository
    {
        Task<int> AddKRAAsync(Kra kra);
        Task AddKRADescriptionsAsync(IEnumerable<TblDescription> descriptions);
    }
}