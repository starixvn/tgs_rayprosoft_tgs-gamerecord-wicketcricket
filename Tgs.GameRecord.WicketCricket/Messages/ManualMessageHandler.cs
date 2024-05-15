using System.Text;
using System.Text.Json;
using Tgs.GameRecord.WicketCricket.Common;
using Tgs.GameRecord.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.WicketCricket.Messages;

internal class ManualMessageHandler(
	ILogger<ManualMessageHandler> logger,
	ICommandService<PullRecordCommand> service) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var json = Encoding.UTF8.GetString(data.Span);
		var message = JsonSerializer.Deserialize<WicketCricketGameRecordMessage>(json)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("LogOn:{logOn} Subject:{subject} Information Request:{message}",
			DateTime.Now.ToString("o"),
			subject,
			message);

		var command = new PullRecordCommand(
			ApiUrl: message.ApiUrl,
			Website: message.Website,
			Cert: message.Cert,
			Agent: message.Agent,
			StartTime: message.StartTime,
			EndTime: message.EndTime,
			ReportType: message.ReportType);

		return service.ExecuteAsync(
			command: command,
			cancellationToken: cancellationToken);
	}
}
