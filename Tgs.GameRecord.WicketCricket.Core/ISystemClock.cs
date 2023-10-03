namespace Tgs.GameRecord.WicketCricket.Core;

public interface ISystemClock
{
    DateTimeOffset CurrentTime { get; }
}
