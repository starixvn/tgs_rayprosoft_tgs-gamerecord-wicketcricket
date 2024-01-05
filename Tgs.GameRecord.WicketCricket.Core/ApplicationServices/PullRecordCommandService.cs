using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.WicketCricket.Core.ApplicationServices;

internal class PullRecordCommandService(
    ILogger<PullRecordCommandService> logger,
    ICommandService<UnsettledCommand> unsettledService,
    ICommandService<SettledCommand> settledService) : ICommandService<PullRecordCommand>
{
    public async ValueTask ExecuteAsync(PullRecordCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var unsettledCommand = new UnsettledCommand(
                ApiUrl: command.ApiUrl,
                Website: command.Website,
                Cert: command.Cert,
                Agent: command.Agent,
                ReportType: command.ReportType,
                LastUpdateTime: command.StartTime);

            await unsettledService.ExecuteAsync(
                command: unsettledCommand,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "LogOn:{logOn} Message: Unsettled exception.({command})", DateTime.Now.ToString("o"), command);
        }

        try
        {
            var settledCommand = new SettledCommand(
                ApiUrl: command.ApiUrl,
                Website: command.Website,
                Cert: command.Cert,
                Agent: command.Agent,
                ReportType: command.ReportType,
                StartTime: command.StartTime,
                EndTime: command.EndTime);

            await settledService.ExecuteAsync(
                command: settledCommand,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "LogOn:{logOn} Message: Settled exception.({command})", DateTime.Now.ToString("o"), command);
        }
    }
}
