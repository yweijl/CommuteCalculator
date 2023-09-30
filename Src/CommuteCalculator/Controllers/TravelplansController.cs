using AutoMapper;
using CommuteCalculator.Dto.Travelplans.Requests;
using CommuteCalculator.Dto.Travelplans.Responses;
using CommuteCalculator.Extensions;
using Core.Interfaces;
using Core.Models.Travelplans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CommuteCalculator.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TravelplansController : ControllerBase
{
    private readonly ITravelplanService _travelplanService;
    private readonly IUserTravelplanService _userTravelplanService;
    private readonly IMapper _mapper;

    public TravelplansController(ITravelplanService travelplanService, IUserTravelplanService userTravelPlanService, IMapper mapper)
    {
        _travelplanService = travelplanService;
        this._userTravelplanService = userTravelPlanService;
        this._mapper = mapper;
    }

    [HttpPost(Name = "CalculateTravelplan")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<RouteRegistrationResponse>))]
    public async Task<IActionResult> CalculateTravelPlan([FromBody] CalculateTravelplanRequest request)
    {
        var waypoints =_mapper.Map<List<WayPoints>>(request.Waypoints);
        var travelplan = await _travelplanService.CalculateTravelplanAsync(this.User.GetUserId(), waypoints);

        var travelplanResponse = _mapper.Map<List<RouteRegistrationResponse>>(travelplan);
        return Ok(travelplanResponse);
    }

    [HttpGet(Name = "GetUserTravelplans")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserTravelplanResponse))]
    public async Task<IActionResult> GetUserTravelplans()
    {
        var travelplans = await _userTravelplanService.GetPersistedTravelplansAsync(this.User.GetUserId());
        var response = _mapper.Map<UserTravelplanResponse>(travelplans);
        return Ok(response);
    }

    [HttpDelete(Name = "DeleteTravelplan")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task <IActionResult> DeleteTravelplan(Guid travelplanId)
    {
        var deleted = await _userTravelplanService.DeleteTravelplanByIdAsync(travelplanId, this.User.GetUserId());
        return deleted ? NoContent() : BadRequest();
    }

    [HttpGet("download/{monthNumber}")]
    [Produces("application/octet-stream")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FileContentResult))]
    public async Task<IActionResult> DownloadTravelplan(int monthNumber)
    {
        var travelplan = await _userTravelplanService.DownloadTravelplanAsync(monthNumber, this.User.GetUserId());
        return File(travelplan.Item2, "application/octet-stream", @$"{travelplan.Item1}.csv");
    }

    [HttpPost("save")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> PersistTravelplan([FromBody] PersistTravelplanRequest persistTravelplanRequest)
    {
        var request = _mapper.Map<Travelplan>(persistTravelplanRequest);
        var persisted = await _userTravelplanService.SaveAsync(this.User.GetUserId(), request);
        return persisted ? NoContent() : NotFound();
    }
}