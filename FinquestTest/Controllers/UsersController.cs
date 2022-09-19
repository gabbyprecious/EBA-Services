using System;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

using AutoMapper;

using FinquestTest.Models;
using FinquestTest.Services;
using Microsoft.AspNetCore.Mvc;



namespace FinquestTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UsersService _usersService;
    private readonly IMessageService _messageService;
    //private IMapper _mapper;

    public UserController(UsersService usersService, IMessageService messageService)
    {
        _usersService = usersService;
        _messageService = messageService;
        //_mapper = mapper;
    }
        
    


    [HttpGet]
    public async Task<List<User>> Get() =>
        await _usersService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        //var user = _mapper.Map<User>(newUser);
        await _usersService.CreateAsync(newUser);
        var messageData = Newtonsoft.Json.JsonConvert.SerializeObject(newUser);

        _messageService.Enqueue(messageData);

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Authenticate([FromBody] Login model)
    {
        var user = await _usersService.LoginAsync(model.Username, model.Password);

        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });
        return user;
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await _usersService.UpdateAsync(id, updatedUser);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _usersService.RemoveAsync(id);

        return NoContent();
    }
}
