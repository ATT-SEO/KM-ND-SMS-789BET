using API.KM58.Data;
using API.KM58.Model;
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
		public WorkerService(IServiceProvider ServiceProvider)
		{
			_dbContext = ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            _boService = ServiceProvider.CreateScope().ServiceProvider.GetService<IBOService>();
        }

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				while (true)
				{
                    //Get the new SMSRawData list
                    List<SMSRawData> _listSMSRawData = await _dbContext.SMSRawData.Where(x => x.Status == false).ToListAsync();
					for (int i = 0; i < _listSMSRawData.Count; i++)
					{
						SMSRawData _tempSMSRawData = _listSMSRawData[i];
						Console.WriteLine(_tempSMSRawData.Sender + "|" + _tempSMSRawData.Content + "|" + _tempSMSRawData.ProjectID + "|" + _tempSMSRawData.Device);

                        //Maching with data in the request list
                        SMS _targetSMS = await _dbContext.SMS.Where(x => x.Sender ==_tempSMSRawData.Sender && x.Content ==_tempSMSRawData.Content).FirstOrDefaultAsync();
                        if (_targetSMS!=null && _targetSMS.CreatedTime==null && _targetSMS.Status==false)
                        {
                            //Found the request
                            //Todo 
                            //#1 - Prepare to update full information for Request
                            Site site = _dbContext.Sites.First(u => u.Project==_tempSMSRawData.ProjectID && u.Status==true);
                            if (site!=null)
                            {
                                Random rnd = new Random();
                                int Point = rnd.Next(site.MinPoint, site.MaxPoint + 1);
                                string mySite = site.Name;
                                string Account = _targetSMS.Account;
                                int Round = site.Round;
                                string Remarks = site.Remarks;
                                string Ecremarks = site.Ecremarks;

                                JObject jsonResponseData;
                                if (mySite.Trim() == "mocbai")
                                {
                                    var jsonAllJiLiFishTickets = await _boService.addPointClient(mySite, Account, Point, Round, Remarks, Ecremarks);
                                    jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());
                                }
                                else
                                {
                                    var jsonAllJiLiFishTickets = await _boService.addPointClientCMD(mySite, Account, Point, Round, Remarks, Ecremarks);
                                    jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());
                                }

                                if (jsonResponseData != null && jsonResponseData["status_code"] != null && (int)jsonResponseData["status_code"] == 200)
                                {
                                    //Update Request To Done
                                    _targetSMS.CreatedTime = DateTime.Now;
                                    _targetSMS.ProjectCode = _tempSMSRawData.ProjectID;
                                    _targetSMS.Point = Point;
                                    _targetSMS.Status = true;
                                    _dbContext.Entry(_targetSMS).State = EntityState.Modified;

                                    
                                    Log.Information("UPDATE_SUCCESS[" + _tempSMSRawData.Sender + "][" + _tempSMSRawData.Content + "]");
                                }
                                else
                                {
                                    Log.Information("UPDATE_FAILED[" + _tempSMSRawData.Sender + "][" + _tempSMSRawData.Content + "]");
                                }
                            }else
                            {
                                Log.Information("CANNOT_FOUND_CONFIG[" + _tempSMSRawData.Sender + "][" + _tempSMSRawData.Content + "]");
                            }
                        }
                        else
                        {
                            Log.Information("CANNOT_FOUND_REQUEST["+ _tempSMSRawData.Sender + "]["+ _tempSMSRawData.Content + "]");
                        }

                        _tempSMSRawData.Status = true;
                        _dbContext.Update(_tempSMSRawData);
                        await _dbContext.SaveChangesAsync();
                    }
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
				}
			}
			catch (Exception ex)
			{
				Log.Information("ExecuteAsync - " + ex.Message);
			}
			return;
		}
	}
}
