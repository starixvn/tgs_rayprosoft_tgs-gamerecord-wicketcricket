using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.WicketCricket.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.WicketCricket.Controllers;

[Route("9WCRICKET")]
[ApiController]
public class WicketCricketController : ControllerBase
{
    /// <summary>
    /// Games the record manual asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
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
    ///             "agent": "testpginrma",
    ///             "startTime": "2023-09-27T00:00:00+08:00",
    ///             "endTime": "2023-09-28T00:00:00+08:00",
    ///             "reportType": "FancyBet"
    ///         }
    /// </remarks>
    [HttpPost("manual")]
    public ValueTask GameRecordManualAsync(
        [FromServices] IMessageSender messageSender,
        WicketCricketGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: WicketCricketNatsConst.ManualPublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the record schedule asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
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
    ///             "agent": "testpginrma",
    ///             "startTime": "2023-09-27T00:00:00+08:00",
    ///             "endTime": "2023-09-28T00:00:00+08:00",
    ///             "reportType": "FancyBet"
    ///         }
    /// </remarks>
    [HttpPost("schedule")]
    public ValueTask GameRecordScheduleAsync(
        [FromServices] IMessageSender messageSender,
        WicketCricketGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: WicketCricketNatsConst.SchedulePublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);
}
