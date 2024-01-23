using Players.Application.Contracts.Logging;
using Players.Application.Players.Contracts;
using Players.Application.Common.Exceptions;
using MediatR;

namespace Players.Application.Players.Commands;

public sealed class HealCommand : IRequest
{
	public required Guid Id { get; init; }
	public required int Health { get; init; }
}

internal sealed class HealCommandHandler : IRequestHandler<HealCommand>
{
	private readonly IPlayerRepository ctx;
	private readonly ILoggerAdapter<HealCommandHandler> logger;

	public HealCommandHandler(IPlayerRepository ctx, ILoggerAdapter<HealCommandHandler> logger)
	{
		this.ctx = ctx;
		this.logger = logger;
	}

	public async Task Handle(HealCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Healing player");

		var player = await ctx.LoadAsync(request.Id, cancellationToken);

		if (player is null)
		{
			logger.LogInformation("Player with Id: {Id} was not found", request.Id);
			throw new NotFoundException("Player with was not found");
		}

		player.Heal(request.Health);
		await ctx.StoreAsync(player, cancellationToken);
	}
}
