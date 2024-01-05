using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Tgs.GameInterface.WicketCricket.Client;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.GameRecord.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.WicketCricket.Core.UnitTest.ApplicationServices;

public class UnsettledCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLogger = NullLogger<UnsettledCommandService>.Instance;
        var fakeClient = Substitute.For<IGameInterfaceWicketCricketClient>();
        var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

        var command = new UnsettledCommand(
            ApiUrl: new("http://localhost/"),
            Website: "Website",
            Cert: "Cert",
            Agent: "agent",
            ReportType: ReportTypes.Exchange,
            LastUpdateTime: DateTimeOffset.UtcNow.AddMinutes(-10));

        var service = new UnsettledCommandService(
            fakeLogger,
            fakeClient,
            fakeService);

        await service.ExecuteAsync(command: command);

        _ = fakeClient
            .Received(1)
            .GameWagerUnsettledAsync(
            apiUrl: Arg.Is(command.ApiUrl),
            website: Arg.Is(command.Website),
            cert: Arg.Is(command.Cert),
            lastUpdateTime: Arg.Is(command.LastUpdateTime),
            reportType: Arg.Is(command.ReportType),
            pageNumber: Arg.Any<int>(),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
