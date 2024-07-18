using Tgs.GameRecord.WicketCricket.Core.JobApplicationServices;

namespace Tgs.GameRecord.WicketCricket.Core;

public interface IHangfireUtensil
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}
