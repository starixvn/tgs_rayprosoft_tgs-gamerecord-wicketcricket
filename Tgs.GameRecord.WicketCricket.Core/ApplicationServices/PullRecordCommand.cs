using Tgs.GameInterface.WicketCricket.Protos;

namespace Tgs.GameRecord.WicketCricket.Core.ApplicationServices;
public record PullRecordCommand(
    Uri ApiUrl,
    string Website,
    string Cert,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    ReportTypes ReportType);
