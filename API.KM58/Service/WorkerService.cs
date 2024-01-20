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
					Log.Information("SMS_INCOMMING - "+_listSMSRawData.Count + " - "+DateTime.Now);
					for (int i = 0; i < _listSMSRawData.Count; i++)
					{
						SMSRawData _tempSMSRawData = _listSMSRawData[i];
						Log.Information("SMS_RAW_DATA => ["+_tempSMSRawData.Sender + "|" + _tempSMSRawData.Content + "|" + _tempSMSRawData.ProjectID + "|" + _tempSMSRawData.Device+"]");

                        //Maching with data in the request list
                        SMS _targetSMS = await _dbContext.SMS.Where(x => x.Sender ==_tempSMSRawData.Sender && x.Content ==_tempSMSRawData.Content && x.Status==false).FirstOrDefaultAsync();
                        if (_targetSMS!=null && _targetSMS.CreatedTime==null && _targetSMS.Status==false)
                        {
							//Found the request
                            Log.Information(JsonConvert.SerializeObject(_targetSMS));
							Log.Information("FOUND_REQUEST => ["+_targetSMS.Account+" - "+_targetSMS.ProjectCode+"]");
							//Todo 
							//#1 - Prepare to update full information for Request
							Site site = _dbContext.Sites.Where(u => u.Project==_targetSMS.ProjectCode && u.Status==true).FirstOrDefault();
							if (site!=null)
                            {
                                Random rnd = new Random();
                                int Point = rnd.Next(site.MinPoint, site.MaxPoint + 1);
                                string mySite = site.Name;
                                string Account = _targetSMS.Account;
                                int Round = site.Round;
                                string Remarks = site.Remarks;
                                string Ecremarks = site.Ecremarks;
                                Log.Information("ADD POINT => ["+ Point +"]");
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
                                    Log.Information("****DONE****");
                                }
                                else
                                {
                                    Log.Information("****FAILED****[" + _tempSMSRawData.Sender + "][" + _tempSMSRawData.Content + "]");
                                }
                            }else
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
			}
			catch (Exception ex)
			{
				Log.Information("ExecuteAsync - " + ex.Message);
			}
			return;
		}
	}
}
