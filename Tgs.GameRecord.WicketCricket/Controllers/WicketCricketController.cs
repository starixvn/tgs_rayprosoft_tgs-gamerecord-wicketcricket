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
	[HttpPost("manual")]
	public ValueTask GameRecordManualAsync(
		[FromServices] IMessageSender messageSender,
		WicketCricketGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: WicketCricketNatsConst.ManualPublish,
			data: JsonSerializer.SerializeToUtf8Bytes(source),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record schedule asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("schedule")]
	public ValueTask GameRecordScheduleAsync(
		[FromServices] IMessageSender messageSender,
		WicketCricketGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: WicketCricketNatsConst.SchedulePublish,
			data: JsonSerializer.SerializeToUtf8Bytes(source),
			cancellationToken: HttpContext.RequestAborted);
}
