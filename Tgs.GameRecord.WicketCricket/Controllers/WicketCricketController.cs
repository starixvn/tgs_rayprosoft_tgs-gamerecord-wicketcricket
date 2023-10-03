using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tgs.GameRecord.WicketCricket.Core;
using Tgs.GameRecord.WicketCricket.ViewModels;
using Tgs.GameRounds.Schedule.Client;
using Tgs.GameRounds.Schedule.Protos;

namespace Tgs.GameRecord.WicketCricket.Controllers;

[Route("9WCRICKET")]
[ApiController]
public class WicketCricketController : ControllerBase
{
    private readonly GameRecordInfoOptions _gameRecordInfo;

    public WicketCricketController(
        IOptions<GameRecordInfoOptions> options)
    {
        _gameRecordInfo = options.Value; ;
    }

    /// <summary>
    /// Manuals the game record asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "agent": "zf055gpi",
    ///             "startTime": "2023-09-27T00:00:00+08:00",
    ///             "endTime": "2023-09-28T00:00:00+08:00",
    ///             "reportType": 7
    ///         }
    /// </remarks>
    [HttpPost("manual")]
    public ValueTask ManualGameRecordAsync(
        [FromServices] IGameRoundsScheduleClient client,
        WicketCricketViewModel source)
        => client.WicketCricket_ScheduleFormQueuedAsync(
            gameProviderCode: _gameRecordInfo.GameProviderCode,
            name: $"{source.Website}-{source.ReportType}",
            subject: "gp.gamerecord.scheduleform.wicketCricket.queued",
            source: ScheduleFormSource.Manual,
            apiUrl: source.ApiUrl,
            website: source.Website,
            cert: source.Cert,
            agent: string.Empty,
            startTime: source.StartTime,
            endTime: source.EndTime,
            reportType: source.ReportType.ToString(),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Schedules the game record asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "agent": "zf055gpi",
    ///             "startTime": "2023-09-27T00:00:00+08:00",
    ///             "endTime": "2023-09-28T00:00:00+08:00",
    ///             "reportType": 7
    ///         }
    /// </remarks>
    [HttpPost("schedule")]
    public ValueTask ScheduleGameRecordAsync(
        [FromServices] IGameRoundsScheduleClient client,
        WicketCricketViewModel source)
        => client.WicketCricket_ScheduleFormQueuedAsync(
            gameProviderCode: _gameRecordInfo.GameProviderCode,
            name: $"{source.Website}-{source.ReportType}",
            subject: "gp.gamerecord.scheduleform.wicketCricket.queued",
            source: ScheduleFormSource.Schedule,
            apiUrl: source.ApiUrl,
            website: source.Website,
            cert: source.Cert,
            agent: string.Empty,
            startTime: source.StartTime,
            endTime: source.EndTime,
            reportType: source.ReportType.ToString(),
            cancellationToken: HttpContext.RequestAborted);
}
