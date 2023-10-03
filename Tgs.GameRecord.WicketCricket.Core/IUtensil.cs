namespace Tgs.GameRecord.WicketCricket.Core;

public interface IUtensil
{
    bool TimeDifferenceWithin2Hours(DateTimeOffset source);
}
