using Microsoft.Extensions.Logging;
using Tgs.GameInterface.WicketCricket.Client;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.WicketCricket.Core.ApplicationServices;

internal class UnsettledCommandService : ICommandService<UnsettledCommand>
{
    private readonly IGameInterfaceWicketCricketClient _client;
    private readonly ILogger<UnsettledCommandService> _logger;
    private readonly ICommandService<HandleRecordCommand> _service;

    public UnsettledCommandService(
        ILogger<UnsettledCommandService> logger,
        IGameInterfaceWicketCricketClient client,
        ICommandService<HandleRecordCommand> service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask ExecuteAsync(UnsettledCommand command, CancellationToken cancellationToken = default)
    {
        var pageNumber = 1;
        var lastUpdateTime = command.LastUpdateTime;
        IEnumerable<GameWagerInfo> infos;
        do
        {
            infos = await _client
                .GameWagerUnsettledAsync(
                apiUrl: command.ApiUrl,
                website: command.Website,
                cert: command.Cert,
                lastUpdateTime: lastUpdateTime,
                reportType: command.ReportType,
                pageNumber: pageNumber,
                cancellationToken: cancellationToken)
                .ToArrayAsync(
                cancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameWagerInfo>();

            pageNumber++;

            if (infos.Any())
            {
                lastUpdateTime = infos.Max(x => x.ExternalCreatedOn.ToDateTimeOffset());
            }

            foreach (var info in infos)
            {
                _logger.LogDebug("LogOn:{logOn} Message: GameWager. GameWager:{info}", DateTime.Now.ToString("o"), info);

                try
                {
                    await _service
                        .ExecuteAsync(
                            command: new(info),
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LogOn:{logOn} Message: GameWager Exception. GameWager:{info}", DateTime.Now.ToString("o"), info);
                }
            }
        } while (infos.Any());
    }
}
