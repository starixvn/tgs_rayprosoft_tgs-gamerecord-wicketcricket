using Tgs.GameInterface.WicketCricket.Protos;

namespace Tgs.GameRecord.WicketCricket.Core.JobApplicationServices;

public record JobExecuteCommand(
    string GameProviderCode,
    string Name,
    Uri ApiUrl,
    string Website,
    string Cert,
    string Agent,
    ReportTypes ReportType);
