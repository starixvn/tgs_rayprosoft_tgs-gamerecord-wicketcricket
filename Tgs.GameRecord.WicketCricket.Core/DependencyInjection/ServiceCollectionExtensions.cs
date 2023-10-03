using System.Reflection;
using Tgs.GameCommand.Client;
using Tgs.GameInterface.WicketCricket.Client;
using Tgs.GameRecord.WicketCricket.Core;
using Tgs.GameRounds.Schedule.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRecordCore(
        this IServiceCollection services,
        Action<RedisOptions, IServiceProvider> redisDBconfigure,
        Action<GameRecordInfoOptions, IServiceProvider> gameRecordInfoOptions,
        Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
        Action<GameRoundsScheduleClientOptions, IServiceProvider> gameRoundsScheduleClientOptions,
        Action<GameInterfaceWicketCricketClientOptions, IServiceProvider> gameInterfaceWicketCricketClientOptions)
        => services
        .AddOptions<RedisOptions>()
        .Configure(redisDBconfigure)
        .Services
        .AddOptions<GameRecordInfoOptions>()
        .Configure(gameRecordInfoOptions)
        .Services
        .RegisterAllServiceUtilityTypes(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Transient)
        .AddSingleton<RedisConnectionManager>()
        .AddGameCommandClient(gameCommandClientOptions)
        .AddGameRoundsScheduleClient(gameRoundsScheduleClientOptions)
        .AddGameInterfaceWicketCricketClient(gameInterfaceWicketCricketClientOptions)
        .AddGameRecordHandleClient()
        .AddTransient<IHangfireUtensil, HangfireUtensil>()
        .AddSingleton<IScheduleFormDispatcher, ScheduleFormDispatcher>()
        .AddSingleton<IScheduleFormLock>(sp => ActivatorUtilities.CreateInstance<ScheduleFormLock>(sp, sp.GetRequiredService<RedisConnectionManager>().Database))
        .AddSingleton<IUtensil, UtensilRepository>()
        .AddSingleton<ISystemClock, SystemClock>();
}
