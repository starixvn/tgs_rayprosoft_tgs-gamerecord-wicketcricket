using System.ComponentModel.DataAnnotations;
using Tgs.GameInterface.WicketCricket.Protos;

namespace Tgs.GameRecord.WicketCricket.Common;

public record WicketCricketGameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	public string Website { get; init; } = default!;
	[Required]
	public string Cert { get; init; } = default!;
	[Required]
	public string Agent { get; init; } = default!;
	[Required]
	public ReportTypes ReportType { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	public DateTimeOffset StartTime { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	public DateTimeOffset EndTime { get; init; } = default!;
}
