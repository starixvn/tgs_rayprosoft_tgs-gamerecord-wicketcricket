using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.WicketCricket.Core.ApplicationServices;

internal class PullRecordCommandService : ICommandService<PullRecordCommand>
{
    private readonly ILogger<PullRecordCommandService> _logger;
    private readonly ICommandService<UnsettledCommand> _unsettledService;
    private readonly ICommandService<SettledCommand> _settledService;

    public PullRecordCommandService(
        ILogger<PullRecordCommandService> logger,
        ICommandService<UnsettledCommand> unsettledService,
        ICommandService<SettledCommand> settledService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unsettledService = unsettledService ?? throw new ArgumentNullException(nameof(unsettledService));
        _settledService = settledService ?? throw new ArgumentNullException(nameof(settledService));
    }

    public async ValueTask ExecuteAsync(PullRecordCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var unsettledCommand = new UnsettledCommand(
                ApiUrl: command.ApiUrl,
                Website: command.Website,
                Cert: command.Cert,
                LastUpdateTime: command.StartTime,
                ReportType: command.ReportType);

            await _unsettledService.ExecuteAsync(
                command: unsettledCommand,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LogOn:{logOn} Message: Unsettled exception.({command})", DateTime.Now.ToString("o"), command);
        }

        try
        {
            var settledCommand = new SettledCommand(
                ApiUrl: command.ApiUrl,
                Website: command.Website,
                Cert: command.Cert,
                StartTime: command.StartTime,
                EndTime: command.EndTime,
                ReportType: command.ReportType);

            await _settledService.ExecuteAsync(
                command: settledCommand,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LogOn:{logOn} Message: Settled exception.({command})", DateTime.Now.ToString("o"), command);
        }
    }
}
