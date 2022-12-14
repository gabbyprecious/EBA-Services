using System;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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

    public UserController(UsersService usersService, IMessageService messageService)
    {
        _usersService = usersService;
        _messageService = messageService;
        //_mapper = mapper;
    }

    //public UserController(UsersService usersService)
    //{
    //    _usersService = usersService;
    //    //_messageService = messageService;
    //}

    [HttpGet]
    public async Task<List<ListResponseUser>> Get()
    {
        string firstName = this.Request.Query["firstName"];
        string lastName = this.Request.Query["lastName"];
        string atleastAConnection = this.Request.Query["atleastAConnection"];
        string orderBy = this.Request.Query["orderBy"];
        string order = this.Request.Query["order"];
        List<User> users =await _usersService.ListAsync(firstName, lastName, atleastAConnection, order, orderBy);
        List<ListResponseUser> responseUsers = new List<ListResponseUser>();

        foreach (var user in users)
        {
            ListResponseUser responseUser = new ListResponseUser(user.Id, user.FirstName, user.LastName, user.LastConnectionDate);
            responseUsers.Add(responseUser);
        }

        return responseUsers;
    }
        

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

        newUser.Username = newUser.Username.ToLower();

        var user = await _usersService.GetByUsernameAsync(newUser.Username);

        if (user != null)
            return BadRequest(new { message = "Username already exist" });

        await _usersService.CreateAsync(newUser);

        DumpUser dumpUser = new DumpUser(newUser.Id, newUser.FirstName, newUser.LastName, newUser.Username);
        var messageData = Newtonsoft.Json.JsonConvert.SerializeObject(dumpUser);

        _messageService.EnqueueCreate(messageData);

        return CreatedAtAction(nameof(Get), new { message = "User has registered successfully" });
    }

    [HttpPost("login")]
    public async Task<ActionResult<ListResponseUser>> Authenticate([FromBody] Login model)
    {
        model.Username = model.Username.ToLower();

        var user = await _usersService.LoginAsync(model.Username, model.Password);

        ListResponseUser responseUser = new ListResponseUser(user.Id, user.FirstName, user.LastName, user.LastConnectionDate);

        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });
        return responseUser;
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult<User>> Update(string id, UpdateModel updatedUser)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _usersService.UpdateAsync(user, updatedUser);

        var messageData = Newtonsoft.Json.JsonConvert.SerializeObject(user);
        _messageService.EnqueueUpdate(messageData);
        return user;
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
