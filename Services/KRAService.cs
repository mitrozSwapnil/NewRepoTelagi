using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Services
{
    public class KRAService : IKRAService
    {
        private readonly IKRARepository _kraRepository;

        public KRAService(IKRARepository kraRepository)
        {
            _kraRepository = kraRepository;
        }

        public async Task GenerateKRAAsync(KraViewModel dto)
        {
            // Create KRA
            var kra = new Kra
            {
                KraName = dto.KraName,
                Quarter = dto.Quarter,
                IsTarget = dto.KeepTarget,
                KraStartDate = dto.KraStartDate,
                KraEndDate = dto.KraEndDate,
                IsDelete = 0,
                FkUserId = 1,

                CreatedAt = DateTime.UtcNow
            };

            int kraId = await _kraRepository.AddKRAAsync(kra);

            // Create Descriptions
            var descriptions = dto.KraDescriptions.Select(desc => new TblDescription
            {
                KraIdfk = kraId,
                Description = desc
            });

            await _kraRepository.AddKRADescriptionsAsync(descriptions);
        }
    }

    //public class KRAService : IKRAService
    //{
    //    private readonly IKRARepository _kraRepository;

    //    public KRAService(IKRARepository kraRepository)
    //    {
    //        _kraRepository = kraRepository;
    //    }

    //    public async Task GenerateKRAAsync(KraViewModel dto)
    //    {
    //        var kra = new TblKra
    //        {
    //            KraName = dto.KraName,
    //            Quarter = dto.Quarter,
    //            IsTarget = dto.KeepTarget
    //        };
    //        int kraId = await _kraRepository.AddKRAAsync(kra);//send request to Repository

    //        var descriptions = dto.KraDescriptions.Select(desc => new KRADescription
    //        {
    //            KRAId = kraId,
    //            Description = desc
    //        });

    //        await _kraRepository.AddKRADescriptionsAsync((IEnumerable<tblKraDescription>)descriptions);
    //    }
    //}
}