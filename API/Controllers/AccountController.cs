using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _dataContext;

    public AccountController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost("register")] // POST: api/account/register
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username))
        {
            return BadRequest("Username is taken");
        }
        
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            Username = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        return user;
    }

    private async Task<bool> UserExists(string username)
    {
        return await _dataContext.Users.AnyAsync(x => x.Username == username.ToLower());
    }
}