using System;
using TelgeProject.Entity;
using TelgeProject.Interface;

namespace TelgeProject.Repository
{
    public class KRARepository : IKRARepository
    {
        private readonly db_telgeprojectContext _context;

        public KRARepository(db_telgeprojectContext context)
        {
            _context = context;
        }

        public async Task<int> AddKRAAsync(Kra kra)
        {
            await _context.Kras.AddAsync(kra);
            await _context.SaveChangesAsync();
            return kra.Id;
        }

        public async Task AddKRADescriptionsAsync(IEnumerable<TblDescription> descriptions)
        {
            await _context.TblDescriptions.AddRangeAsync(descriptions);
            await _context.SaveChangesAsync();
        }
    }

    //public class KRARepository : IKRARepository
    //{
    //    private readonly db_telgeprojectContext _context;

    //    public KRARepository(db_telgeprojectContext context)
    //    {
    //        _context = context;
    //    }
    //    public async Task<int> AddKRAAsync(TblKra kra)
    //    {
    //        _context.TblKras.Add(kra);
    //        await _context.SaveChangesAsync();
    //        return kra.Id;
    //    }

    //    public async Task AddKRADescriptionsAsync(IEnumerable<tblKraDescription> descriptions)
    //    {
    //        _context.tblKraDescriptions.AddRange(descriptions);
    //        await _context.SaveChangesAsync();
    //    }


    //}
}