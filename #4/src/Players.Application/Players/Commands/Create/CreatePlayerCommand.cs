using Players.Application.Contracts.Logging;
using Players.Application.Players.Contracts;
using Players.Domain.Players;
using MediatR;

namespace Players.Application.Players.Commands;

public sealed record CreatePlayerCommand : IRequest<Guid>
{
	public required string Name { get; init; }
	public required int Health { get; init; }
}

internal sealed class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Guid>
{
	private readonly IPlayerRepository ctx;
	private readonly ILoggerAdapter<CreatePlayerCommandHandler> logger;

	public CreatePlayerCommandHandler(IPlayerRepository ctx, ILoggerAdapter<CreatePlayerCommandHandler> logger)
	{
		this.ctx = ctx;
		this.logger = logger;
	}

	public async Task<Guid> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Creating player");

		var player = new Player(Guid.NewGuid(), request.Name, request.Health);

		await ctx.StoreAsync(player, cancellationToken);

		return player.Id;
	}
}
