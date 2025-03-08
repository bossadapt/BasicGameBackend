
using System.Diagnostics.CodeAnalysis;

[method: SetsRequiredMembers]
public class Player(string username, string password)
{
    public string Id { get; } = Guid.NewGuid().ToString();

    public required string Username { get; set; } = username;
    public required string Password { get; set; } = password;
    public DateTime AccountCreated { get; set; } = DateTime.Now;
    public DateTime LastLoggedIn { get; set; } = DateTime.Now;
    public  ICollection<Play> Plays { get; set;} = [];
}