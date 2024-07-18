using Tgs.GameCommand.Client;
using Tgs.GameInterface.WicketCricket.Client;
using Tgs.GameRecord.WicketCricket;
using Tgs.GameRecord.WicketCricket.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameRecord(
		this IServiceCollection services,
		Action<RedisOptions, IServiceProvider> redisDBconfigure,
		Action<GameRecordInfoOptions, IServiceProvider> gameRecordInfoOptions,
		Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
		Action<GameInterfaceWicketCricketClientOptions, IServiceProvider> gameInterfaceWicketCricketClientOptions)
		=> services
		.AddOptions<RedisOptions>()
		.Configure(redisDBconfigure)
		.Services
		.AddOptions<GameRecordInfoOptions>()
		.Configure(gameRecordInfoOptions)
		.Services
		.AddSingleton<RedisConnectionManager>()
		.AddGameRecordCore(
			gameCommandClientOptions: gameCommandClientOptions,
			gameInterfaceWicketCricketClientOptions: gameInterfaceWicketCricketClientOptions,
			redisDatabaseResolver: (sp) => sp.GetRequiredService<RedisConnectionManager>().Database);
}
