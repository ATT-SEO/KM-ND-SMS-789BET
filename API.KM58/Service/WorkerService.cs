using Microsoft.Extensions.Hosting;

namespace API.KM58.Service
{
	public class WorkerService : BackgroundService
	{
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Console.WriteLine("WorkerService is running.");
			return;
		}
	}
}
