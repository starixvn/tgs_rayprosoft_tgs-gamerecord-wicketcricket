using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.GameRecord.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.WicketCricket.Core.UnitTest.ApplicationServices;

public class PullRecordCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLogger = NullLogger<PullRecordCommandService>.Instance;
        var fakeUnsettledService = Substitute.For<ICommandService<UnsettledCommand>>();
        var fakeSettledService = Substitute.For<ICommandService<SettledCommand>>();


        var command = new PullRecordCommand(
            ApiUrl: new("http://localhost/"),
            Website: "Website",
            Cert: "Cert",
            StartTime: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndTime: DateTimeOffset.UtcNow,
            ReportType: ReportTypes.Exchange);


        var service = new PullRecordCommandService(
            fakeLogger,
            fakeUnsettledService,
            fakeSettledService);

        await service.ExecuteAsync(command: command);


        _ = fakeUnsettledService
            .Received(1)
            .ExecuteAsync(
            command: Arg.Is(new UnsettledCommand(
                ApiUrl: command.ApiUrl,
                Website: command.Website,
                Cert: command.Cert,
                LastUpdateTime: command.StartTime,
                ReportType: command.ReportType)),
            cancellationToken: Arg.Any<CancellationToken>());


        _ = fakeSettledService
            .Received(1)
            .ExecuteAsync(
            command: Arg.Is(new SettledCommand(
                ApiUrl: command.ApiUrl,
                Website: command.Website,
                Cert: command.Cert,
                StartTime: command.StartTime,
                EndTime: command.EndTime,
                ReportType: command.ReportType)),
            cancellationToken: Arg.Any<CancellationToken>());

    }
}
