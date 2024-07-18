using Hangfire.Dashboard;

namespace Tgs.GameRecord.WicketCricket;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context) => true;
}
