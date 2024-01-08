using API.KM58.Data;
using API.KM58.Service.IService;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.KM58.Service
{
    public class BackgroundWorkerService : IBackgroundWorkerService
    {
        private readonly IBOService _boService;
        private readonly AppDbContext _db;
        public BackgroundWorkerService(AppDbContext DB, IBOService boService)
        {
            _boService = boService;
            _db = DB;
        }

        //public async Task<string> DoWork_RoutineJob()
        //{
        //    try
        //    {
        //        List<PromotionClaim> PromotionClaimErrorList = await _db.PromotionClaims
        //            .Include(u => u.TargetList)
        //            .Where(x => x.ResultStatus == 2)
        //            .ToListAsync();
        //        for (int i = 0; i < PromotionClaimErrorList.Count; i++)
        //        {
        //            PromotionClaim tempPromotionClaim = PromotionClaimErrorList[i];
        //            long currentTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
        //            long updateTime = ((DateTimeOffset)tempPromotionClaim.UpdateTime).ToUnixTimeMilliseconds();
        //            Console.WriteLine("CURRENT TIME  - " + (currentTime - updateTime));
        //            if (currentTime - updateTime > 2 * 60 * 1000)
        //            {
        //                _db.PromotionClaims.Remove(tempPromotionClaim);
        //            }
        //            _db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return "";
        //}
    }
}
