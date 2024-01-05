using Microsoft.Extensions.Logging;
using Tgs.GameInterface.WicketCricket.Client;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.WicketCricket.Core.ApplicationServices;

internal class UnsettledCommandService(
    ILogger<UnsettledCommandService> logger,
    IGameInterfaceWicketCricketClient client,
    ICommandService<HandleRecordCommand> service) : ICommandService<UnsettledCommand>
{
    public async ValueTask ExecuteAsync(UnsettledCommand command, CancellationToken cancellationToken = default)
    {
        var pageNumber = 1;
        var lastUpdateTime = command.LastUpdateTime;
        IEnumerable<GameWagerInfo> infos;
        do
        {
            infos = await client
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
