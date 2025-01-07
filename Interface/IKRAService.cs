using TelgeProject.Models;

namespace TelgeProject.Interface
{
    public interface IKRAService
    {
        Task GenerateKRAAsync(KraViewModel dto);
    }
}