using System.Reflection;
using StackExchange.Redis;
using Tgs.GameCommand.Client;
using Tgs.GameInterface.WicketCricket.Client;
using Tgs.GameRecord.WicketCricket.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRecordCore(
        this IServiceCollection services,
        Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
        Action<GameInterfaceWicketCricketClientOptions, IServiceProvider> gameInterfaceWicketCricketClientOptions,
        Func<IServiceProvider, IDatabase> redisDatabaseResolver)
        => services
        .RegisterAllServiceUtilityTypes(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Transient)
        .AddGameCommandClient(gameCommandClientOptions)
        .AddGameInterfaceWicketCricketClient(gameInterfaceWicketCricketClientOptions)
        .AddTransient<IHangfireUtensil, HangfireUtensil>()
        .AddTransient<IHangfireLock>(sp => ActivatorUtilities.CreateInstance<HangfireLock>(sp, redisDatabaseResolver(sp)));
}
