using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Tgs.GameInterface.WicketCricket.Client;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.GameRecord.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.WicketCricket.Core.UnitTest.ApplicationServices;

public class SettledCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLogger = NullLogger<SettledCommandService>.Instance;
        var fakeClient = Substitute.For<IGameInterfaceWicketCricketClient>();
        var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

        var command = new SettledCommand(
            ApiUrl: new("http://localhost/"),
            Website: "Website",
            Cert: "Cert",
            StartTime: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndTime: DateTimeOffset.UtcNow,
            ReportType: ReportTypes.Exchange);

        var service = new SettledCommandService(
            fakeLogger,
            fakeClient,
            fakeService);

        await service.ExecuteAsync(command: command);

        _ = fakeClient
            .Received(1)
            .GameWagerSettledAsync(
            apiUrl: Arg.Is(command.ApiUrl),
            website: Arg.Is(command.Website),
            cert: Arg.Is(command.Cert),
            startTime: Arg.Is(command.StartTime),
            endTime: Arg.Is(command.EndTime),
            reportType: Arg.Is(command.ReportType),
            pageNumber: Arg.Any<int>(),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
