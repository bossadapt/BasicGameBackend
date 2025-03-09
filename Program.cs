using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Diagnostics;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PlayerContext>(options =>
    options.UseSqlite("Data Source=players.db"));
//AUTH 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });
builder.Services.AddAuthorization();

var app = builder.Build();
//More auth
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

app.UseCookiePolicy(cookiePolicyOptions);
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-6.0
app.UseHttpsRedirection();
//basic auth
app.MapPost("/register", async (PlayerLogin loginAttempt, PlayerContext db) =>
{
    if (string.IsNullOrWhiteSpace(loginAttempt.Username) || string.IsNullOrWhiteSpace(loginAttempt.Password))
    {
        return Results.BadRequest();
    }
    var existingName = await Task.FromResult(db.Players.Where(player => player.Username == loginAttempt.Username).FirstOrDefault());
    if (existingName != null)
    {
        return Results.Conflict("username already in use");
    }
    else
    {
        var passwordHasher = new PasswordHasher<Player>();
        string hashedPassword = passwordHasher.HashPassword(null, loginAttempt.Password);
        db.Players.Add(new Player(loginAttempt.Username, hashedPassword));
        await db.SaveChangesAsync();
        return Results.Ok("Player created successfully!");
    }
})
.WithName("CreatePlayer")
.WithOpenApi();

app.MapPost("/login", async (PlayerLogin loginAttempt, PlayerContext db, HttpContext httpContext) =>
{
    if (loginAttempt == null || string.IsNullOrEmpty(loginAttempt.Username) || string.IsNullOrEmpty(loginAttempt.Password))
    {
        return Results.BadRequest("missing username or password");
    }
    var playerResult = await Task.FromResult(db.Players.Where(player => player.Username == loginAttempt.Username).FirstOrDefault());
    if (playerResult == null)
    {
        return Results.Unauthorized();
    }
    var passwordHasher = new PasswordHasher<Player>();
    var verificationResult = passwordHasher.VerifyHashedPassword(playerResult, playerResult.Password, loginAttempt.Password);

    if (verificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
    {
        return Results.Unauthorized();
    }
    else
    {
        //update last signed in
        await db.Players
    .Where(x => x.Id == playerResult.Id)
    .ExecuteUpdateAsync(setters => setters.SetProperty(player => player.LastLoggedIn, DateTime.Now));
        //pass claims
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, playerResult.Username),
        new Claim(ClaimTypes.NameIdentifier, playerResult.Id)
    };
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = true,
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity),
               authProperties);
        return Results.Ok(new { Message = "Login successful!" });
    }
})
.WithName("login")
.WithOpenApi();

app.MapGet("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(
         CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok("Player logged out!");

})
.WithName("LogOutPlayer")
.WithOpenApi();

app.MapGet("/player", async (HttpContext httpContext, PlayerContext db) =>
{
    if (httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
    {
        return Results.Unauthorized();
    }
    var playerResult = await Task.FromResult(db.Players.Where(player => player.Id == httpContext.User.Identity.GetUserId()).FirstOrDefault());
    if (playerResult == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(new {playerResult.Id,playerResult.Username,playerResult.LastLoggedIn,playerResult.AccountCreated});
    }


})
.WithName("GetPlayer")
.WithOpenApi();
app.MapPost("/play", async (string mapId, float timeLength, HttpContext httpContext, PlayerContext db) =>
{
    if (httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
    {
        return Results.Unauthorized();
    }
    var player = await db.Players.FirstOrDefaultAsync(p => p.Id == httpContext.User.Identity.GetUserId());
    var map = await db.Maps.FirstOrDefaultAsync(p => p.Id == mapId);
    if (player == null)
    {
        return Results.NotFound("Invalid Player ID");
    }
    else if (map == null)
    {
        return Results.NotFound("Invalid Map ID");
    }
    else
    {
        var newPlay = new Play(player.Id, mapId, timeLength);
        player.Plays.Add(newPlay);
        await db.SaveChangesAsync();
        return Results.Ok("play added");
    }


})
.WithName("addPlay")
.WithOpenApi();


app.MapGet("/plays", async (HttpContext httpContext, PlayerContext db, string playerId = null) =>
{
    ICollection<Play>? playerResult = null;
    //grabed by another player
    if (string.IsNullOrEmpty(playerId))
    {
        if (httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            return Results.BadRequest();
        }
        playerResult = await Task.FromResult(db.Players.Where(player => player.Id == httpContext.User.Identity.GetUserId()).Select(player => player.Plays).FirstOrDefault());
    }
    else
    {

        playerResult = await Task.FromResult(db.Players.Where(player => player.Id == playerId).Select(player => player.Plays).FirstOrDefault());
    }
    if (playerResult == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(playerResult);
    }


})
.WithName("GetPlays")
.WithOpenApi();

app.MapGet("/map", async (string mapId, PlayerContext db) =>
{
    var map = await db.Maps.FirstOrDefaultAsync(p => p.Id == mapId);
    if (map == null)
    {
        return Results.NotFound("Map Not Found");
    }
    else
    {
        string sqlQuery =@"
            WITH RankedPlays AS (
                SELECT *, 
                    ROW_NUMBER() OVER (PARTITION BY playerId ORDER BY PlayLength ASC) AS rank
                FROM Plays
                WHERE mapId = '{@MapId}'
            )
            SELECT * 
            FROM RankedPlays
            WHERE rank = 1
            ORDER BY PlayLength ASC
            LIMIT 100;
        ";
        var top100 = db.Plays.FromSqlRaw(string.Format(sqlQuery,map.Id)).AsNoTracking();
        return Results.Ok(new { map.Id, map.AuthorTime, map.SPlusTime, map.STime, map.ATime, map.BTime, top100 });
    }
})
.WithName("GetMap")
.WithOpenApi();
app.MapGet("/leaderboard", async (string mapId,int startIndex,int endIndex ,PlayerContext db) =>
{
    if(startIndex>endIndex){
        return Results.BadRequest("Start index is lower than end index");
    }
    var map = await db.Maps.FirstOrDefaultAsync(p => p.Id == mapId);
    if (map == null)
    {
        return Results.NotFound("Map Not Found");
    }
    else
    {
        var sqlQuery = @"
            WITH RankedPlays AS (
                SELECT *, 
                    ROW_NUMBER() OVER (PARTITION BY playerId ORDER BY PlayLength ASC) AS rank
                FROM Plays
                WHERE mapId = '{0}'
            )
            SELECT * 
            FROM RankedPlays
            WHERE rank = 1
            ORDER BY PlayLength ASC
            LIMIT {1} OFFSET {2};
        ";
        var top100 = db.Plays.FromSqlRaw(string.Format(sqlQuery,map.Id,(endIndex-startIndex),startIndex)).AsNoTracking();
        return Results.Ok(new { map.Id, map.AuthorTime, map.SPlusTime, map.STime, map.ATime, map.BTime, top100 });
    }
}).WithName("GetLeaderboardByIndex")
.WithOpenApi();
app.MapGet("/maps", async (PlayerContext db) =>
{
    var maps = await Task.FromResult(db.Maps.Select(map => new { map.Id, map.AuthorTime, map.SPlusTime, map.STime, map.ATime, map.BTime }).ToArray());
    if (maps.Length == 0)
    {
        return Results.NoContent();
    }
    else
    {
        return Results.Ok(maps);
    }
})
.WithName("GetMaps")
.WithOpenApi();



app.Run();
