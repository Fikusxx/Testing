using Players.Application.Players.Commands;
using Players.Application.Players.Queries;
using Players.Domain.Players.Enums;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Players.Api.Controllers;

[ApiController]
[Route("players")]
public sealed class PlayersController : ControllerBase
{
	private readonly IMediator mediator;

	public PlayersController(IMediator mediator)
	{
		this.mediator = mediator;
	}

	[HttpGet]
	[Route("{playerId:guid}")]
	public async Task<IActionResult> Get([FromRoute] Guid playerId)
	{
		var query = new GetByIdQuery() { Id = playerId };
		var result = await mediator.Send(query);
		var dto = new PlayerDto()
		{
			Id = result.Id,
			Name = result.Name.Value,
			Health = result.Health.Value,
			Gold = result.Gold.Value,
			Status = result.Status
		};

		return Ok(dto);
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> Post([FromBody] CreatePlayerCommand command)
	{
		var result = await mediator.Send(command);	

		return Ok(result);
	}

	[HttpPut]
	[Route("name")]
	public async Task<IActionResult> ChangeName([FromBody] ChangeNameCommand command)
	{
		await mediator.Send(command);

		return NoContent();
	}

	[HttpPut]
	[Route("heal")]
	public async Task<IActionResult> Heal([FromBody] HealCommand command)
	{
		await mediator.Send(command);

		return NoContent();
	}

	[HttpPut]
	[Route("gold")]
	public async Task<IActionResult> SpendGold([FromBody] SpendGoldCommand command)
	{
		await mediator.Send(command);

		return NoContent();
	}
}

public class PlayerDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }	
	public int Health { get; set; }
	public int Gold { get; set; }
	public Status Status { get; set; }
}