namespace Tgs.GameRecord.WicketCricket.Core;

public interface IHangfireLock
{
    public Task<bool> LockingAsync(string application, CancellationToken cancellationToken = default);
}
