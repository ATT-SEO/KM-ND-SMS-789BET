using API.KM58.Data;
using API.KM58.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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
					List<SMS> _listSMS = await _dbContext.SMS.Where(x => x.Status == false).ToListAsync();
					Console.WriteLine(JsonConvert.SerializeObject(_listSMS));

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
