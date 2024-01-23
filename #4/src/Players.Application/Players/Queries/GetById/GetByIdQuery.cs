using Players.Application.Players.Contracts;
using Players.Application.Contracts.Logging;
using Players.Application.Common.Exceptions;
using Players.Domain.Players;
using MediatR;

namespace Players.Application.Players.Queries;

public sealed record GetByIdQuery : IRequest<Player>
{
	public required Guid Id { get; init; }
}

internal sealed class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Player>
{
	private readonly IPlayerRepository ctx;
	private readonly ILoggerAdapter<GetByIdQueryHandler> logger;

	public GetByIdQueryHandler(IPlayerRepository ctx, ILoggerAdapter<GetByIdQueryHandler> logger)
	{
		this.ctx = ctx;
		this.logger = logger;
	}

	public async Task<Player> Handle(GetByIdQuery request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Getting player");

		var player = await ctx.LoadAsync(request.Id, cancellationToken);

		if (player is null)
		{
			logger.LogInformation("Player with Id: {Id} was not found", request.Id);
			throw new NotFoundException($"Player with Id: {request.Id} was not found");
		}

		return player;
	}
}
