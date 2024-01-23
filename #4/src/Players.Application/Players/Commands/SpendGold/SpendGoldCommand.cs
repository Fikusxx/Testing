using Players.Application.Common.Exceptions;
using Players.Application.Contracts.Logging;
using Players.Application.Players.Contracts;
using MediatR;

namespace Players.Application.Players.Commands;

public sealed class SpendGoldCommand : IRequest
{
	public required Guid Id { get; init; }
	public required int Gold { get; init; }
}

internal sealed class SpendGoldCommandHandler : IRequestHandler<SpendGoldCommand>
{
	private readonly IPlayerRepository ctx;
	private readonly ILoggerAdapter<SpendGoldCommandHandler> logger;

	public SpendGoldCommandHandler(IPlayerRepository ctx, ILoggerAdapter<SpendGoldCommandHandler> logger)
	{
		this.ctx = ctx;
		this.logger = logger;
	}

	public async Task Handle(SpendGoldCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Spending gold");

		var player = await ctx.LoadAsync(request.Id, cancellationToken);

		if (player is null)
		{
			logger.LogInformation("Player with Id: {Id} was not found", request.Id);
			throw new NotFoundException("Player with was not found");
		}

		player.SpendGold(request.Gold);
		await ctx.StoreAsync(player, cancellationToken);
	}
}
