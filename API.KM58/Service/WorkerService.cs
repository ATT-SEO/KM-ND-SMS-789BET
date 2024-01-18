using API.KM58.Data;
using API.KM58.Model;
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
		public WorkerService(IServiceProvider ServiceProvider)
		{
			_dbContext = ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				while (true)
				{
					List<SMSRawData> _listSMSRawData = await _dbContext.SMSRawData.Where(x => x.Status == false).ToListAsync();
					for (int i = 0; i < _listSMSRawData.Count; i++)
					{
						SMSRawData _tempSMSRawData = _listSMSRawData[i];
						Console.WriteLine(_tempSMSRawData.Sender + "|" + _tempSMSRawData.Content + "|" + _tempSMSRawData.ProjectID + "|" + _tempSMSRawData.Device);
						
						SMS _targetSMS = await _dbContext.SMS.Where(x=>
																	x.Sender ==_tempSMSRawData.Sender && 
																	x.Content ==_tempSMSRawData.Content).FirstOrDefaultAsync();

                        //if (_targetSMS != null && _targetSMS.CreatedTime == null)
                        //{
                        //    Site site = _dbContext.Sites.First(u => u.Project == _tempSMSRawData.ProjectID);

                        //    Random rnd = new Random();
                        //    int Point = rnd.Next(site.MinPoint, site.MaxPoint + 1);
                        //    string Site = site.Name;
                        //    string Account = _targetSMS.Account;
                        //    int Round = site.Round;
                        //    string Remarks = site.Remarks;
                        //    string Ecremarks = site.Ecremarks;

                        //    JObject jsonResponseData;

                        //    if (Site.Trim() == "mocbai")
                        //    {
                        //        var jsonAllJiLiFishTickets = await _boService.addPointClient(Site, Account, Point, Round, Remarks, Ecremarks);
                        //        jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());

                        //    }
                        //    else
                        //    {
                        //        var jsonAllJiLiFishTickets = await _boService.addPointClientCMD(Site, Account, Point, Round, Remarks, Ecremarks);
                        //        jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());
                        //    }

                        //    if (jsonResponseData != null && jsonResponseData["status_code"] != null && (int)jsonResponseData["status_code"] == 200)
                        //    {
                        //        existingSMS.CreatedTime = DateTime.Now;
                        //        existingSMS.ProjectCode = inputSMS.ProjectCode;
                        //        existingSMS.Point = Point;
                        //        existingSMS.Status = true;
                        //        _db.Entry(existingSMS).State = EntityState.Modified;
                        //        await _db.SaveChangesAsync();
                        //        _response.Result = existingSMS;
                        //        _response.IsSuccess = true;
                        //        _response.Message = "Cập nhật điểm thành công.";
                        //    }
                        //    else
                        //    {
                        //        _response.IsSuccess = false;
                        //        _response.Message = "Lỗi cộng điểm vào bo";
                        //    }
                        //}
                        //else
                        //{
                        //    _response.IsSuccess = false;
                        //    _response.Message = "SMS không hợp lệ cơ sở dữ liệu.";
                        //}


                        //SMS _targetSMS = _dbContext.SMS.Where(x=>x.sen)
                    }
					await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
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
