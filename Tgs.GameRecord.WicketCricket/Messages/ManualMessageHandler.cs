using System.Text;
using System.Text.Json;
using Tgs.GameRecord.WicketCricket.Common;
using Tgs.GameRecord.WicketCricket.Core;
using Tgs.GameRecord.WicketCricket.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.WicketCricket.Messages;

internal class ManualMessageHandler(
	ILogger<ManualMessageHandler> logger,
	IPullRecordDispatcher dispatcher) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var json = Encoding.UTF8.GetString(data.Span);
		var message = JsonSerializer.Deserialize<WicketCricketGameRecordMessage>(json)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("{logInformation}",
			JsonSerializer.Serialize(new
			{
				LogOn = DateTime.Now.ToString("u"),
				Subject = subject,
				Data = new
				{
					message.ApiUrl,
					message.Website,
					message.Cert,
					message.Agent,
					StartTime = message.StartTime.ToString("u"),
					EndTime = message.EndTime.ToString("u"),
					message.ReportType
				},
				Message = "This receive message handler."
			}));

		return dispatcher.PushCommandAsync(
			command: new PullRecordCommand(
				ApiUrl: message.ApiUrl,
				Website: message.Website,
				Cert: message.Cert,
				Agent: message.Agent,
				StartTime: message.StartTime,
				EndTime: message.EndTime,
				ReportType: message.ReportType),
			cancellationToken: cancellationToken);
	}
}
