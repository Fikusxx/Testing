using Players.Application.Contracts.Persistence;
using Players.Domain.Players;

namespace Players.Application.Players.Contracts;

public interface IPlayerRepository : IAggregateRootRepository<Player>
{ }
