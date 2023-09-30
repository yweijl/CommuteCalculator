using AutoMapper;
using CommuteCalculator.Dto.Contacts;
using CommuteCalculator.Extensions;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CommuteCalculator.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ILogger<ContactsController> _logger;
    private readonly IContactService _contactService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;


    public ContactsController(ILogger<ContactsController> logger, IContactService contactService, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _logger = logger;
        _contactService = contactService;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ContactResponse))]
    public async Task<IActionResult> Get(Guid id)
    {
        var contact = await _contactService.GetByIdAsync(id);
        var response = _mapper.Map<ContactResponse>(contact); 
        return contact == null
            ? NotFound()
            : Ok(response);
    }

    [HttpGet("list")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<ContactResponse>))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> ListByUserId()
    {
        var contacts = await _contactService.ListByUserIdAsync(this.User.GetUserId());
        var response = _mapper.Map<List<ContactResponse>>(contacts);

        return Ok(response);
    }

    [HttpDelete()]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> RemoveById(Guid contactId)
    {
        var removed = await this._contactService.DeleteByIdAsync(contactId);
        return removed ? NoContent() : BadRequest();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ContactResponse))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Add([FromBody] AddContactRequest request)
    {
        var contact = _mapper.Map<Contact>(request);
        contact.Id = this.User.GetUserId();

        var addedContact = await _contactService.SaveContactAsync(contact);

        var response = _mapper.Map<ContactResponse>(addedContact);

        return CreatedAtAction(nameof(Get), new { id = addedContact.Id }, addedContact);
    }
}