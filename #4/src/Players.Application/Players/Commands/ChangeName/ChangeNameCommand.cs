using Players.Application.Players.Contracts;
using Players.Application.Common.Exceptions;
using Players.Application.Contracts.Logging;
using MediatR;

namespace Players.Application.Players.Commands;

public sealed record ChangeNameCommand : IRequest
{
	public required Guid Id { get; init; }
	public required string Name { get; init; }
}

internal sealed class ChangeNameCommandHandler : IRequestHandler<ChangeNameCommand>
{
	private readonly IPlayerRepository ctx;
	private readonly ILoggerAdapter<ChangeNameCommandHandler> logger;

	public ChangeNameCommandHandler(IPlayerRepository ctx, ILoggerAdapter<ChangeNameCommandHandler> logger)
	{
		this.ctx = ctx;
		this.logger = logger;
	}

	public async Task Handle(ChangeNameCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Changing name to {Name}", request.Name);

		var player = await ctx.LoadAsync(request.Id, cancellationToken);

		if (player is null)
		{
			logger.LogInformation("Player with Id: {Id} was not found", request.Id);
			throw new NotFoundException("Player with was not found");
		}

		player.ChangeName(request.Name);
		await ctx.StoreAsync(player, cancellationToken);
	}
}
