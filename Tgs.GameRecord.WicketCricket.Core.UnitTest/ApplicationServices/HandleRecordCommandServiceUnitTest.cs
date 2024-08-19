using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.WicketCricket.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.WicketCricket.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Loss()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "9WCRICKET",
			GameExternalId = "sports_book",
			GameProviderConfiguration = "9WCRICKETConfiguration",
			HangfirePrefix = "gamerecord-wicketCricket-hangfire:",
			HangfireLocking = "gamerecord-wicketCricket-locking:",
			IntervalMinutes = 30d,
			IntervalMinutesMax = 1440d,
			IntervalMinutesMinimum = 1d
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			new GameRecordInfo
			{
				ExternalRoundId = "-10020446",
				ExternalBetId = "2_96935586",

				ExternalGameId = "4",
				PlayerCode = "4ue0ujotpmuu",
				IsClosed = true,
				IsCancelled = false,
				IsVoided = false,
				ExternalCreatedOn = DateTimeOffset.Parse("2023-09-27T09:37:00.176+08:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-09-27T10:11:07.332+08:00").ToTimestamp(),
				Bet = 1000m,
				Settle = 0,
				WinLoss = -1000m,
				AdditionalData = "{\"betPlaced\":\"2023-09-27T09:37:00.176\\u002B08:00\",\"matchSettledDate\":\"2023-09-27T10:11:07.331\\u002B08:00\",\"marketSettledDate\":\"2023-09-27T10:11:07.332\\u002B08:00\",\"eventTypeName\":\"CRICKET\",\"eventName\":\"AutoFancyBookMakerResettle v 09-27 09\",\"eventId\":-10020446,\"site\":44,\"currencyType\":2,\"eventType\":4,\"selectionId\":-1,\"categoryType\":2,\"ip\":\"114.33.41.237\",\"isp\":\"Chunghwa Telecom Co. Ltd.\",\"matchOddsReq\":100.00,\"betId\":96935586,\"userId\":\"4ue0ujotpmuu\",\"extension1\":null,\"agentId\":\"testpginrma\",\"oddsType\":1,\"updateDate\":1695780804242,\"marketName\":\"FANCY_BET\",\"selectionName\":\"first inning 10 over runs\",\"matchType\":\"No\",\"marketId\":\"46514\",\"fancyBetRuns\":10,\"fancyBetExtendedRuns\":0,\"fancyBetMarketType\":1,\"transactionData\":[{\"betTaken\":\"2023-09-27T09:37:00.176\\u002B08:00\",\"txStake\":1000.000000,\"txLiability\":-1000.0000000000000000,\"txOddsMatched\":100.00,\"actualOdds\":100.00,\"txFancyBetRuns\":10}],\"matchAvgOdds\":100.00,\"betStatus\":\"Settled\",\"profitLoss\":-1000.000000,\"reduction\":0,\"matchStake\":1000.000000}"
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Info.PlayerCode,
				ExternalRoundId = command.Info.ExternalBetId,
				ExternalBetId = command.Info.ExternalBetId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Loss,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				ValidBet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				Settle = command.Info.Settle.ToDecimal().GetValueOrDefault(0),
				Commission = 0.0m,
				Turnover = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				WinLoss = command.Info.WinLoss.ToDecimal().GetValueOrDefault(0),
				Jackpot = 0.0M,
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Running()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "9WCRICKET",
			GameExternalId = "sports_book",
			GameProviderConfiguration = "9WCRICKETConfiguration",
			HangfirePrefix = "gamerecord-wicketCricket-hangfire:",
			HangfireLocking = "gamerecord-wicketCricket-locking:",
			IntervalMinutes = 30d,
			IntervalMinutesMax = 1440d,
			IntervalMinutesMinimum = 1d
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			new GameRecordInfo
			{
				ExternalRoundId = "-10020457",
				ExternalBetId = "2_96935587",
				ExternalGameId = "4",
				PlayerCode = "4ue0ujotpmuu",
				IsClosed = false,
				IsCancelled = false,
				IsVoided = false,
				ExternalCreatedOn = DateTimeOffset.Parse("2023-09-27T11:35:53.213+08:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.UnixEpoch.ToTimestamp(),
				Bet = 1000m,
				Settle = 0.0m,
				WinLoss = 0.0m,
				AdditionalData = "{\"eventTypeName\":\"CRICKET\",\"eventName\":\"\",\"eventId\":30465936,\"site\":44,\"currencyType\":2,\"eventType\":4,\"selectionId\":-1,\"categoryType\":2,\"ip\":\"103.55.10.179\",\"isp\":\"Ivacy Limited\",\"matchOddsReq\":100.00,\"betId\":91407284,\"userId\":\"4ue0ujotpmuu\",\"extension1\":null,\"agentId\":\"testwg681ma\",\"oddsType\":1,\"updateDate\":1620012031779,\"betPlaced\":\"2023-09-27T11:35:53.213u002B08:00\",\"matchSettledDate\":\"\",\"marketName\":\"FANCY_BET\",\"selectionName\":\"first inning 15 over runs\",\"matchType\":\"Yes\",\"marketId\":\"26001\",\"fancyBetRuns\":88,\"fancyBetExtendedRuns\":0,\"fancyBetMarketType\":1,\"transactionData\":null,\"matchAvgOdds\":100.00,\"betStatus\":\"Not Settled\",\"matchAmount\":1000.000000,\"unMatchAmount\":0,\"dateMatched\":\"2023-09-27T11:35:53.213u002B08:00\"}"
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Info.PlayerCode,
				ExternalRoundId = command.Info.ExternalBetId,
				ExternalBetId = command.Info.ExternalBetId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = DateTimeOffset.UnixEpoch,
				ExternalPayoffOn = DateTimeOffset.UnixEpoch,
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Running,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				ValidBet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				Settle = 0.0m,
				Commission = 0.0m,
				Turnover = 0.0m,
				WinLoss = 0.0m,
				Jackpot = 0.0M,
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Void()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "9WCRICKET",
			GameExternalId = "sports_book",
			GameProviderConfiguration = "9WCRICKETConfiguration",
			HangfirePrefix = "gamerecord-wicketCricket-hangfire:",
			HangfireLocking = "gamerecord-wicketCricket-locking:",
			IntervalMinutes = 30d,
			IntervalMinutesMax = 1440d,
			IntervalMinutesMinimum = 1d
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			new GameRecordInfo
			{
				ExternalRoundId = "-10020452",
				ExternalBetId = "2_96935585",
				ExternalGameId = "4",
				PlayerCode = "4ue0ujotpmuu",
				IsClosed = true,
				IsCancelled = false,
				IsVoided = true,
				ExternalCreatedOn = DateTimeOffset.Parse("2023-09-27T09:36:13.017+08:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-09-27T10:20:52.303+08:00").ToTimestamp(),
				Bet = 1000m,
				Settle = 0.0m,
				WinLoss = 0.0m,
				AdditionalData = "{\"betPlaced\":\"2023-09-27T09:36:13.017\\u002B08:00\",\"matchSettledDate\":\"2023-09-27T10:20:52.302\\u002B08:00\",\"marketSettledDate\":\"2023-09-27T10:20:52.303\\u002B08:00\",\"eventTypeName\":\"CRICKET\",\"eventName\":\"AutoFancyBookMakerVoid v 09-27 09\",\"eventId\":-10020452,\"site\":44,\"currencyType\":2,\"eventType\":4,\"selectionId\":-1,\"categoryType\":2,\"ip\":\"114.33.41.237\",\"isp\":\"Chunghwa Telecom Co. Ltd.\",\"matchOddsReq\":100.00,\"betId\":96935585,\"userId\":\"4ue0ujotpmuu\",\"extension1\":null,\"agentId\":\"testpginrma\",\"oddsType\":1,\"updateDate\":1695781368979,\"marketName\":\"FANCY_BET\",\"selectionName\":\"first inning 10 over runs\",\"matchType\":\"No\",\"marketId\":\"46515\",\"fancyBetRuns\":10,\"fancyBetExtendedRuns\":0,\"fancyBetMarketType\":1,\"transactionData\":[],\"matchAvgOdds\":0,\"betStatus\":\"Voided\",\"profitLoss\":0,\"reduction\":0,\"matchStake\":1000.000000}"
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Info.PlayerCode,
				ExternalRoundId = command.Info.ExternalBetId,
				ExternalBetId = command.Info.ExternalBetId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Void,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				ValidBet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				Settle = 0.0M,
				Commission = 0.0m,
				Turnover = 0.0M,
				WinLoss = 0.0M,
				Jackpot = 0.0M,
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Win()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "9WCRICKET",
			GameExternalId = "sports_book",
			GameProviderConfiguration = "9WCRICKETConfiguration",
			HangfirePrefix = "gamerecord-wicketCricket-hangfire:",
			HangfireLocking = "gamerecord-wicketCricket-locking:",
			IntervalMinutes = 30d,
			IntervalMinutesMax = 1440d,
			IntervalMinutesMinimum = 1d
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			new GameRecordInfo
			{
				ExternalRoundId = "-10020457",
				ExternalBetId = "2_96935587",
				ExternalGameId = "4",
				PlayerCode = "4ue0ujotpmuu",
				IsClosed = true,
				IsCancelled = false,
				IsVoided = false,
				ExternalCreatedOn = DateTimeOffset.Parse("2023-09-27T11:35:53.213+08:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-09-27T12:00:52.133+08:00").ToTimestamp(),
				Bet = 1000m,
				Settle = 2000,
				WinLoss = 1000m,
				AdditionalData = "{\"betPlaced\":\"2023-09-27T11:35:53.213\\u002B08:00\",\"matchSettledDate\":\"2023-09-27T12:00:52.132\\u002B08:00\",\"marketSettledDate\":\"2023-09-27T12:00:52.133\\u002B08:00\",\"eventTypeName\":\"CRICKET\",\"eventName\":\"AutoTestMarket v 09-27 11\",\"eventId\":-10020457,\"site\":44,\"currencyType\":2,\"eventType\":4,\"selectionId\":-1,\"categoryType\":2,\"ip\":\"114.33.41.237\",\"isp\":\"Chunghwa Telecom Co. Ltd.\",\"matchOddsReq\":100.00,\"betId\":96935587,\"userId\":\"4ue0ujotpmuu\",\"extension1\":null,\"agentId\":\"testpginrma\",\"oddsType\":1,\"updateDate\":1695787252242,\"marketName\":\"FANCY_BET\",\"selectionName\":\"first inning 10 over runs\",\"matchType\":\"No\",\"marketId\":\"46519\",\"fancyBetRuns\":10,\"fancyBetExtendedRuns\":0,\"fancyBetMarketType\":1,\"transactionData\":[{\"betTaken\":\"2023-09-27T11:35:53.213\\u002B08:00\",\"txStake\":1000.000000,\"txLiability\":-1000.0000000000000000,\"txOddsMatched\":100.00,\"actualOdds\":100.00,\"txFancyBetRuns\":10}],\"matchAvgOdds\":100.00,\"betStatus\":\"Settled\",\"profitLoss\":1000.000000,\"reduction\":0,\"matchStake\":1000.000000}"
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Info.PlayerCode,
				ExternalRoundId = command.Info.ExternalBetId,
				ExternalBetId = command.Info.ExternalBetId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 1000m,
				ValidBet = 1000m,
				Settle = 2000m,
				Commission = 0.0m,
				Turnover = 1000m,
				WinLoss = 1000m,
				Jackpot = 0.0M,
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
