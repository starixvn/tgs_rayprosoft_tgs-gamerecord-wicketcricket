using Tgs.GameRecord.WicketCricket.Core.ApplicationServices;

namespace Tgs.GameRecord.WicketCricket.Core;

public interface IPullRecordDispatcher
{
	ValueTask PushCommandAsync(PullRecordCommand command, CancellationToken cancellationToken = default);
}
