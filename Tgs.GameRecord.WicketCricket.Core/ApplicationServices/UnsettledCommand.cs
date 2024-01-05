using Tgs.GameInterface.WicketCricket.Protos;

namespace Tgs.GameRecord.WicketCricket.Core.ApplicationServices;
public record UnsettledCommand(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Agent,
    ReportTypes ReportType,
    DateTimeOffset LastUpdateTime);
