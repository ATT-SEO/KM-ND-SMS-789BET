using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using API.KM58.Service.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace API.KM58.Service
{
    public class WorkerService : BackgroundService
    {
        private AppDbContext _dbContext;
        private IBOService _boService;
        private ICheckConditions _checkConditions;

        public WorkerService(IServiceProvider ServiceProvider)
        {
            _dbContext = ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            _boService = ServiceProvider.CreateScope().ServiceProvider.GetService<IBOService>();
            _checkConditions = ServiceProvider.CreateScope().ServiceProvider.GetService<ICheckConditions>();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (true)
            {
                try
                {
                    //Get the new SMSRawData list
                    List<SMSRawData> _listSMSRawData = await _dbContext.SMSRawData.Where(x => x.Status == false).ToListAsync();
                    Log.Information("SMS_INCOMMING - " + _listSMSRawData.Count + " - " + DateTime.Now);
                    for (int i = 0; i < _listSMSRawData.Count; i++)
                    {
                        SMSRawData _tempSMSRawData = _listSMSRawData[i];
                        Log.Information("SMS_RAW_DATA => [" + _tempSMSRawData.Sender + "|" + _tempSMSRawData.Content + "|" + _tempSMSRawData.ProjectCode + "|" + _tempSMSRawData.Device + "]");
                        //ConvertPhoneNumber.ConvertPhone _tempSMSRawData.Sender
                        //Maching with data in the request list
                        string comparisonPart = _tempSMSRawData.Sender.Substring(3);
                        Log.Information("FOUND_REQUEST SMSSSS DDEN 1 => " + comparisonPart);

                        //x => x.Sender == _tempSMSRawData.Sender
                        SMS _targetSMS = await _dbContext.SMS.Where(
                                                           x => x.Sender.EndsWith(comparisonPart) &&
                                                            x.Content == _tempSMSRawData.Content &&
                                                            x.ProjectCode == _tempSMSRawData.ProjectCode &&
                                                            x.Status == false).FirstOrDefaultAsync();
                        //Log.Information("FOUND_REQUEST 1 => " + _targetSMS.Account);

                        if (_targetSMS != null && _targetSMS.Status == false)
                        {
                            //Found the request
                            //Log.Information(JsonConvert.SerializeObject(_targetSMS));
                            Log.Information("FOUND_REQUEST => [" + _targetSMS.Account + " - " + _targetSMS.ProjectCode + "]");
                            //Todo 
                            //#1 - Prepare to update full information for Request
                            Site site = await _dbContext.Sites.Where(u => u.Project == _targetSMS.ProjectCode && u.Status == true).FirstOrDefaultAsync();
                            if (site != null)
                            {
                                Random rnd = new Random();
                                int Point = rnd.Next(site.MinPoint, site.MaxPoint + 1);
                                string mySite = site.Name;
                                string Account = _targetSMS.Account;
                                int Round = site.Round;
                                string Remarks = site.Remarks;
                                string Ecremarks = site.Ecremarks;
                                Log.Information("ADD POINT SMS  => [" + Point + "]");

                                AccountRegistersDTO accountRegistersDTO = new AccountRegistersDTO
                                {
                                    Account = _targetSMS.Account,
                                    Status = 0,
                                    isSMS = true,
                                    Point = Point,
                                    ProjectCode = site.Project,
                                    Sender = _targetSMS.Sender,
                                    Content = _targetSMS.Content,
                                    Device = _targetSMS.Device,
                                    IP = _targetSMS.IP,
                                    FP = _targetSMS.FP,
                                    AutoPoint = site.AutoPoint
                                };

                                 var CheckAccountRegisters = await _checkConditions.CheckAccountRegisters(accountRegistersDTO, site);
                                 Log.Information(" KTRA CheckAccountRegisters AUTO CHECK SMS  ||   [" + JsonConvert.SerializeObject(@CheckAccountRegisters) + "]");
                                //if (site.AutoPoint == true)
                                //{ 
                                    /// cộng điểm auto
                                    JObject jsonResponseData = null;

                                    //if (site.Project == "FREE66")
                                    //{
                                    //    var jsonAllJiLiFishTickets = await _boService.addPointClient(mySite, Account, Point, Round, Remarks, Ecremarks);
                                    //    jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());
                                    //}

                                    //if (site.Project == "K58")
                                    //{
                                    //    var jsonAllJiLiFishTickets = await _boService.addPointClientCMD(mySite, Account, Point, Round, Remarks, Ecremarks);
                                    //    jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());
                                    //}
                                    
                                //}
                                    

                                if (CheckAccountRegisters.IsSuccess == true) // cập nhật trạng thái của SMS chuyển vào Account Register
                                {
                                    //Update Request To Done
                                    _targetSMS.ProjectCode = _tempSMSRawData.ProjectCode;
                                    _targetSMS.Point = Point;
                                    _targetSMS.Status = true;
                                    _targetSMS.UpdatedTime = DateTime.Now;
                                    _dbContext.Entry(_targetSMS).State = EntityState.Modified;
                                    Log.Information($"KTRA AUTO SMS DONE || {_targetSMS.Account} || {_targetSMS.Content} || {_targetSMS.Point}");
                                }
                                else
                                {
                                    Log.Information("****FAILED****[" + _tempSMSRawData.Sender + "][" + _tempSMSRawData.Content + "]");
                                }
                            }
                            else
                            {
                                Log.Information("CANNOT_FOUND_CONFIG[" + _tempSMSRawData.Sender + "][" + _tempSMSRawData.Content + "]");
                            }
                        }
                        else
                        {
                            Log.Information("=> CANNOT_FOUND_REQUEST");
                        }
                        _tempSMSRawData.Status = true;
                        _dbContext.Update(_tempSMSRawData);
                        await _dbContext.SaveChangesAsync();
                    }
                    Log.Information("========================================================\n");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch (Exception ex)
                {
                    Log.Information("ExecuteAsync - " + ex.Message);
                }
            }
            return;
        }
    }
}
