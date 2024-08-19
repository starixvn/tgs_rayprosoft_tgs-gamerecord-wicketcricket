using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tgs.GameInterface.WicketCricket.Protos;

namespace Tgs.GameRecord.WicketCricket.Common;

public record WicketCricketGameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	[DefaultValue(typeof(Uri), "https://apiinfo.cckk77.net/")]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "WICKETSPRO")]
	public string Website { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "wkVwQ8sMVr252EA9")]
	public string Cert { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "testpginrma")]
	public string Agent { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "FancyBet")]
	public ReportTypes ReportType { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-09-27T00:00:00+08:00")]
	public DateTimeOffset StartTime { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-09-28T00:00:00+08:00")]
	public DateTimeOffset EndTime { get; init; } = default!;
}
