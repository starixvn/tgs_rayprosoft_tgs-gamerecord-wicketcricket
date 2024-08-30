using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.GameRecord.WicketCricket.Common;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.WicketCricket.Core.JobApplicationServices;

internal class JobExecuteCommandService(
	ILogger<JobExecuteCommandService> logger,
	IOptions<GameRecordInfoOptions> options,
	TimeProvider timeProvider,
	IMessageSender messageSender) : ICommandService<JobExecuteCommand>
{
	private readonly double _intervalMinutes = options.Value.IntervalMinutesMax < options.Value.IntervalMinutes
		? options.Value.IntervalMinutesMax
		: options.Value.IntervalMinutesMinimum > options.Value.IntervalMinutes
		? options.Value.IntervalMinutesMinimum
		: options.Value.IntervalMinutes;

	public ValueTask ExecuteAsync(JobExecuteCommand command, CancellationToken cancellationToken = default)
	{
		var currentTime = timeProvider.GetUtcNow();
		var endTime = DateTimeOffset.ParseExact(currentTime.ToString(@"yyyy-MM-dd\THH:mm:00K"), @"yyyy-MM-dd\THH:mm:ssK", null, styles: System.Globalization.DateTimeStyles.RoundtripKind);
		var startTime = endTime.AddMinutes(_intervalMinutes * -1);

		var message = new WicketCricketGameRecordMessage
		{
			ApiUrl = command.ApiUrl,
			Website = command.Website,
			Cert = command.Cert,
			Agent = command.Agent,
			ReportType = command.ReportType,
			StartTime = startTime,
			EndTime = endTime
		};

		logger.LogInformation("{logInformation}",
			JsonSerializer.Serialize(new
			{
				LogOn = DateTime.Now.ToString("u"),
				Subject = WicketCricketNatsConst.SchedulePublish,
				Data = new
				{
					command.ApiUrl,
					command.Website,
					command.Cert,
					command.Agent,
					command.ReportType,
					StartTime = message.StartTime.ToString("u"),
					EndTime = message.EndTime.ToString("u")
				},
				Message = "This message publishing handler."
			}));

		return messageSender.PublishAsync(
			subject: WicketCricketNatsConst.SchedulePublish,
			data: JsonSerializer.SerializeToUtf8Bytes(message),
			cancellationToken: cancellationToken);
	}
}
