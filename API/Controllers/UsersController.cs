using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // GET api/users
public class UsersController : ControllerBase
{
    private readonly DataContext _dataContext;

    public UsersController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        return _dataContext.Users.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUser(int id)
    {
        return _dataContext.Users.Find(id);
    }
}