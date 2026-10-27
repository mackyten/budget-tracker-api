using System.Security.Claims;
using AutoMapper;
using BT.PERSISTENCE.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BT.Tests;

/// <summary>
/// Builds BTDbContext instances over one shared in-memory database.
/// The handlers dispose the context they are given, so every call needs a fresh instance.
/// </summary>
public class TestDb
{
    private readonly string name = Guid.NewGuid().ToString();
    private readonly IHttpContextAccessor accessor;

    public TestDb(string? userId = null, string? email = null)
    {
        var http = new DefaultHttpContext();
        var claims = new List<Claim>();
        if (userId != null) claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
        if (email != null) claims.Add(new Claim(ClaimTypes.Email, email));
        http.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
        accessor = new HttpContextAccessor { HttpContext = http };
    }

    public BTDbContext NewContext() =>
        new(new DbContextOptionsBuilder<BTDbContext>().UseInMemoryDatabase(name).Options, accessor);

    // Mirrors the API's registration (BT.API/Configurations/Mediator.cs): no explicit profiles, inline maps.
    public static IMapper Mapper { get; } =
        new MapperConfiguration(cfg => cfg.ValidateInlineMaps = false).CreateMapper();
}
