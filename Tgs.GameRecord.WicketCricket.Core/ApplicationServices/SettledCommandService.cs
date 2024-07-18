using Microsoft.Extensions.Logging;
using Tgs.GameInterface.WicketCricket.Client;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.WicketCricket.Core.ApplicationServices;

internal class SettledCommandService(
	ILogger<SettledCommandService> logger,
	IGameInterfaceWicketCricketClient client,
	ICommandService<HandleRecordCommand> service) : ICommandService<SettledCommand>
{
	public async ValueTask ExecuteAsync(SettledCommand command, CancellationToken cancellationToken = default)
	{
		var pageNumber = 1;

		IEnumerable<GameRecordInfo> infos;
		do
		{
			infos = await client.GameRecordSettledAsync(
				apiUrl: command.ApiUrl,
				website: command.Website,
				cert: command.Cert,
				startTime: command.StartTime,
				endTime: command.EndTime,
				reportType: command.ReportType,
				pageNumber: pageNumber,
				cancellationToken: cancellationToken)
				.ToArrayAsync(
				cancellationToken)
				.ConfigureAwait(false)
				?? Enumerable.Empty<GameRecordInfo>();
			pageNumber++;

			foreach (var info in infos)
			{
				logger.LogDebug("LogOn:{logOn} Message: GameWager. GameWager:{info}", DateTime.Now.ToString("o"), info);

				try
				{
					await service
						.ExecuteAsync(
							command: new(info),
							cancellationToken: cancellationToken)
						.ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "LogOn:{logOn} Message: GameWager Exception. GameWager:{info}", DateTime.Now.ToString("o"), info);
				}
			}
		} while (infos.Any());
	}
}
